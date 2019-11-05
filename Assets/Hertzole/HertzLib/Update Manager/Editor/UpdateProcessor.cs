#if UNITY_EDITOR
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Hertzole.HertzLib.Editor
{
    [InitializeOnLoad]
    internal static class UpdateProcessor
    {
        private static AssemblyDefinition CurrentAssembly { get; set; }

        static UpdateProcessor()
        {
            CompilationPipeline.assemblyCompilationFinished += OnCompliationFinished;

            //try
            //{
            //    EditorApplication.LockReloadAssemblies();

            //    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            //    {
            //        // Only process assemblies which are in the project
            //        if (assembly.Location.Replace('\\', '/').StartsWith(Application.dataPath.Substring(0, Application.dataPath.Length - 7)))
            //        {
            //            AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assembly.Location);
            //            PostProcessAssembly(assemblyDefinition, assembly.Location);
            //        }
            //    }

            //    EditorApplication.UnlockReloadAssemblies();
            //}
            //catch (Exception ex)
            //{
            //    Debug.LogWarning(ex);
            //}
        }

        private static void OnCompliationFinished(string assemblyPath, CompilerMessage[] messages)
        {
            try
            {
                if (assemblyPath.Contains("-Editor") || assemblyPath.Contains(".Editor"))
                {
                    return;
                }

                string assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);

                //EditorApplication.LockReloadAssemblies();

                //foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                //{
                //    // Only process assemblies which are in the project
                //    if (assembly.Location.Replace('\\', '/').StartsWith(Application.dataPath.Substring(0, Application.dataPath.Length - 7)))
                //    {
                //        AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assembly.Location);
                //        PostProcessAssembly(assemblyDefinition, assembly.Location);
                //    }
                //}

                using (DefaultAssemblyResolver asmResolver = new DefaultAssemblyResolver())
                {
                    using (CurrentAssembly = AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { ReadWrite = true, ReadSymbols = true, AssemblyResolver = asmResolver }))
                    {
                        PostProcessAssembly(CurrentAssembly, assemblyPath);
                        CurrentAssembly.Write();
                    }
                }

                //CompilationPipeline.RequestScriptCompilation();

                //EditorApplication.UnlockReloadAssemblies();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }

        private static void PostProcessAssembly(AssemblyDefinition assembly, string location)
        {
            foreach (ModuleDefinition module in assembly.Modules)
            {
                IEnumerable<TypeDefinition> types = module.GetTypes();
                foreach (TypeDefinition type in types)
                {
                    if (type.Name != "<Module>")
                    {
                        Type realType = Type.GetType(type.FullName + ", " + type.Module.Assembly.FullName);
                        if (realType != null)
                        {
                            if (realType.IsSubclassOf(typeof(MonoBehaviour)) && realType != typeof(UpdateManager) && realType != typeof(UpdateManager.UpdateManagerBehaviour))
                            {
                                ProcessClass(module, type, "Update", "OnUpdate");
                            }
                        }
                    }
                }
            }
        }

        private static void ProcessClass(ModuleDefinition module, TypeDefinition type, string targetUpdate, string convertedUpdate)
        {
            foreach (MethodDefinition method in type.Methods)
            {
                TypeReference updateInterface = null;
                switch (convertedUpdate)
                {
                    case "OnUpdate":
                        updateInterface = module.ImportReference(typeof(IUpdate));
                        break;
                    case "OnFixedUpdate":
                        updateInterface = module.ImportReference(typeof(IFixedUpdate));
                        break;
                    case "OnLateUpdate":
                        updateInterface = module.ImportReference(typeof(ILateUpdate));
                        break;
                    default:
                        throw new ArgumentException("No update function called '" + convertedUpdate + "'.", "convertedUpdate");
                }

                if (method.Name == targetUpdate)
                {
                    MethodDefinition enableMethod = type.Methods.Where(x => x.Name == "OnEnable").FirstOrDefault();
                    MethodDefinition disableMethod = type.Methods.Where(x => x.Name == "OnDisable").FirstOrDefault();

                    bool hasEnableMethod = enableMethod != null;
                    bool hasDisableMethod = disableMethod != null;
                    if (hasEnableMethod || hasDisableMethod)
                    {
                        return;
                    }

                    if (!hasEnableMethod && !hasDisableMethod)
                    {
                        method.Name = convertedUpdate;
                        //method.IsPublic = true;
                    }

                    TypeDefinition updater = GetUpdater(type, module, updateInterface, method);
                    type.NestedTypes.Add(updater);
                    FieldDefinition updaterField = new FieldDefinition("updater", FieldAttributes.Private, updater);
                    type.Fields.Add(updaterField);

                    if (!hasEnableMethod)
                    {
                        enableMethod = new MethodDefinition("OnEnable", MethodAttributes.Private | MethodAttributes.HideBySig, module.ImportReference(typeof(void)));
                        enableMethod.Body.MaxStackSize = 2;
                        enableMethod.Body.Variables.Add(new VariableDefinition(module.ImportReference(typeof(bool))));

                        Instruction addStart = Instruction.Create(OpCodes.Ldarg_0);
                        Instruction endIf = Instruction.Create(OpCodes.Ldloc_0);

                        ILProcessor ilProcessor = enableMethod.Body.GetILProcessor();
                        try
                        {
                            ilProcessor.Emit(OpCodes.Nop);
                            ilProcessor.Emit(OpCodes.Ldarg_0);
                            ilProcessor.Emit(OpCodes.Ldfld, updaterField);
                            ilProcessor.Emit(OpCodes.Ldnull);
                            ilProcessor.Emit(OpCodes.Ceq);
                            ilProcessor.Emit(OpCodes.Stloc_0);
                            ilProcessor.Append(endIf);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning("1");
                            throw;
                        }

                        try
                        {
                            ilProcessor.Emit(OpCodes.Nop);
                            ilProcessor.Emit(OpCodes.Ldarg_0);
                            ilProcessor.Emit(OpCodes.Ldarg_0);

                            MethodDefinition updaterConstructor = updater.Methods.Where(x => x.Name == ".ctor").FirstOrDefault();

                            ilProcessor.Emit(OpCodes.Newobj, updaterConstructor);
                            ilProcessor.Emit(OpCodes.Stfld, updaterField);
                            ilProcessor.Emit(OpCodes.Nop);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning("2 " + ex);
                        }

                        try
                        {
                            ilProcessor.Append(addStart);
                            ilProcessor.Emit(OpCodes.Ldfld, updaterField);
                            ilProcessor.Emit(OpCodes.Call, module.ImportReference(typeof(UpdateManager).GetMethod("Add" + targetUpdate, new Type[] { typeof(IUpdate) })));
                            ilProcessor.Emit(OpCodes.Nop);
                            ilProcessor.Emit(OpCodes.Ret);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning("3");
                        }

                        ilProcessor.InsertAfter(endIf, Instruction.Create(OpCodes.Brfalse_S, addStart));

                        type.Methods.Add(enableMethod);
                    }

                    if (!hasDisableMethod)
                    {
                        disableMethod = new MethodDefinition("OnDisable", MethodAttributes.Private, module.ImportReference(typeof(void)));
                        type.Methods.Add(disableMethod);

                        MethodReference removeUpdateReference = module.ImportReference(typeof(UpdateManager).GetMethod("Remove" + targetUpdate, new Type[] { typeof(IUpdate) }));

                        ILProcessor ilProcessor = disableMethod.Body.GetILProcessor();
                        ilProcessor.Emit(OpCodes.Nop);
                        ilProcessor.Emit(OpCodes.Ldarg_0);
                        ilProcessor.Emit(OpCodes.Ldfld, updaterField);
                        ilProcessor.Emit(OpCodes.Call, removeUpdateReference);
                        ilProcessor.Emit(OpCodes.Nop);
                        ilProcessor.Emit(OpCodes.Ret);
                    }
                    break;
                }
            }
        }

        private static TypeDefinition GetUpdater(TypeDefinition type, ModuleDefinition module, TypeReference updateInterface, MethodReference parentUpdate)
        {
            TypeDefinition updater = new TypeDefinition("", type.Name + "_Updater", TypeAttributes.NestedPrivate);
            FieldDefinition parentField = new FieldDefinition("parent", FieldAttributes.Private, type);
            updater.Fields.Add(parentField);
            updater.Interfaces.Add(new InterfaceImplementation(updateInterface));

            updater.BaseType = module.ImportReference(typeof(object));

            MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
            MethodDefinition constructor = new MethodDefinition(".ctor", methodAttributes, module.ImportReference(typeof(void)));
            MethodReference constructorReference = new MethodReference(".ctor", module.ImportReference(typeof(void)), type.BaseType) { HasThis = true };

            constructor.Parameters.Add(new ParameterDefinition("parent", ParameterAttributes.None, type));

            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Call, constructorReference));
            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Nop));
            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Nop));
            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, parentField));
            constructor.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            updater.Methods.Add(constructor);

            MethodDefinition updateMethod = new MethodDefinition("OnUpdate", MethodAttributes.Public, module.ImportReference(typeof(void)));
            //MethodReference parentUpdateMethod = module.ImportReference(Type.GetType(type.FullName).GetMethod("OnUpdate", Array.Empty<Type>()));

            updateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Nop));
            updateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            updateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, parentField));
            updateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Callvirt, parentUpdate));
            updateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Nop));
            updateMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            updater.Methods.Add(updateMethod);

            return updater;
        }

        private static void ProcessClassOld(ModuleDefinition module, TypeDefinition type, string targetUpdate, string convertedUpdate)
        {
            foreach (MethodDefinition method in type.Methods)
            {
                TypeReference updateInterface = null;
                switch (convertedUpdate)
                {
                    case "OnUpdate":
                        updateInterface = module.ImportReference(typeof(IUpdate));
                        break;
                    case "OnFixedUpdate":
                        updateInterface = module.ImportReference(typeof(IFixedUpdate));
                        break;
                    case "OnLateUpdate":
                        updateInterface = module.ImportReference(typeof(ILateUpdate));
                        break;
                    default:
                        throw new ArgumentException("No update function called '" + convertedUpdate + "'.", "convertedUpdate");
                }

                if (method.Name == targetUpdate)
                {
                    MethodDefinition enableMethod = type.Methods.Where(x => x.Name == "OnEnable").FirstOrDefault();
                    MethodDefinition disableMethod = type.Methods.Where(x => x.Name == "OnDisable").FirstOrDefault();

                    bool hasEnableMethod = enableMethod != null;
                    bool hasDisableMethod = disableMethod != null;
                    if (hasEnableMethod || hasDisableMethod)
                    {
                        return;
                    }

                    InterfaceImplementation existingUpdateInterface = type.Interfaces.Where(x => x.InterfaceType.FullName == updateInterface.FullName).FirstOrDefault();
                    if (existingUpdateInterface == null)
                    {
                        Debug.Log(type.FullName + " needs interface");
                        //type.Interfaces.Add(new InterfaceImplementation(updateInterface));
                        Debug.Log(new InterfaceImplementation(updateInterface).InterfaceType);
                        //type.Interfaces.Add(new InterfaceImplementation(updateInterface));
                        type.Interfaces.Add(new InterfaceImplementation(updateInterface));
                    }

                    if (!hasEnableMethod && !hasDisableMethod)
                    {
                        method.Name = convertedUpdate;
                        method.IsPublic = true;
                    }

                    if (!hasEnableMethod)
                    {
                        enableMethod = new MethodDefinition("OnEnable", MethodAttributes.Private, module.ImportReference(typeof(void)));
                        type.Methods.Add(enableMethod);

                        MethodReference addUpdateReference = module.ImportReference(typeof(UpdateManager).GetMethod("Add" + targetUpdate, new Type[] { typeof(IUpdate) }));

                        ILProcessor ilProcessor = enableMethod.Body.GetILProcessor();
                        ilProcessor.Emit(OpCodes.Nop);
                        ilProcessor.Emit(OpCodes.Ldarg_0);
                        ilProcessor.Append(Instruction.Create(OpCodes.Call, addUpdateReference));
                        ilProcessor.Emit(OpCodes.Nop);
                        ilProcessor.Emit(OpCodes.Ret);
                    }

                    if (!hasDisableMethod)
                    {
                        disableMethod = new MethodDefinition("OnDisable", MethodAttributes.Private, module.ImportReference(typeof(void)));
                        type.Methods.Add(disableMethod);

                        MethodReference removeUpdateReference = module.ImportReference(typeof(UpdateManager).GetMethod("Remove" + targetUpdate, new Type[] { typeof(IUpdate) }));

                        ILProcessor ilProcessor = disableMethod.Body.GetILProcessor();
                        ilProcessor.Emit(OpCodes.Nop);
                        ilProcessor.Emit(OpCodes.Ldarg_0);
                        ilProcessor.Append(Instruction.Create(OpCodes.Call, removeUpdateReference));
                        ilProcessor.Emit(OpCodes.Nop);
                        ilProcessor.Emit(OpCodes.Ret);
                    }
                    break;
                }
            }
        }
    }
}
#endif