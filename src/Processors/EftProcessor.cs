using dnlib.DotNet;
using Spectre.Console;
using TarkovSdkGen.Helpers;
using TarkovSdkGen.UI;

namespace TarkovSdkGen.Processors
{
    public sealed class EftProcessor : AbstractProcessor
    {
        public ProcessorConfig Config { get; } = Program.Config.EFT;

        public EftProcessor() : base(Program.Config.EFT.AssemblyPath, Program.Config.EFT.DumpPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Config.OutputPath, nameof(Config.OutputPath));
        }

        /// <summary>
        /// Run this Processor Job.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Run(StatusContext ctx)
        {
            throw new NotImplementedException("Moved to il2cpp");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"Processing {this.GetType()} entries...");
            var structGenerator_classNames = new StructureGenerator("ClassNames");
            var structGenerator_offsets = new StructureGenerator("Offsets");
            var structGenerator_enums = new StructureGenerator("Enums");
            ProcessClassNames(ctx, structGenerator_classNames);
            ProcessOffsets(ctx, structGenerator_offsets);
            ProcessEnums(ctx, structGenerator_enums);

            AnsiConsole.Clear();

            var sgList = new List<StructureGenerator>()
            {
                structGenerator_classNames,
                structGenerator_offsets,
                structGenerator_enums,
            };
            AnsiConsole.WriteLine(StructureGenerator.GenerateNamespace("SDK", sgList));
            AnsiConsole.WriteLine(StructureGenerator.GenerateReports(sgList));

            string plainSDK = StructureGenerator.GenerateNamespace("SDK", sgList, false);
            File.WriteAllText(Config.OutputPath, plainSDK);
        }

        private void ProcessClassNames(StatusContext ctx, StructureGenerator structGenerator)
        {
            void SetVariableStatus(string variable)
            {
                LastStepName = variable;
                ctx.Status(variable);
            }



            {
                string entity = "get_OpticCameraManager";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("OpticCameraManagerContainer");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }
        }

        private void ProcessOffsets(StatusContext ctx, StructureGenerator structGenerator)
        {
            void SetVariableStatus(string variable)
            {
                LastStepName = variable;
                ctx.Status(variable);
            }

            {
                string name = "GameWorld";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.GameWorld";

                {
                    entity = "Location";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_LocationId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> TransitControllerOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ClientShellingControllerOffset = default;

            {
                string name = "ClientLocalGameWorld";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "ExfilController";

                    // Find the class that has all of these fields
                    List<DumpParser.EntitySearchListEntry> searchEntities = new()
                    {
                        new("EFT.Interactive.ExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                        new("EFT.Interactive.ScavExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                        new("EFT.Interactive.SecretExfiltrations.SecretExfiltrationPoint[]", DumpParser.SearchType.TypeName)
                    };

                    string className = _dumpParser.FindOffsetGroupWithEntities(searchEntities);
                    var offset = _dumpParser.FindOffsetByTypeName(name, className);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "TransitController";

                    var fClass = _dnlibHelper.FindClassWithEntityName("DisableTransitPoints", DnlibHelper.SearchType.Method);
                    TransitControllerOffset = _dumpParser.FindOffsetByTypeName(name, $"-.{fClass.Humanize()}");
                    nestedStruct.AddOffset(entity, TransitControllerOffset);
                }

                {
                    entity = "BtrController";

                    var fClass = _dnlibHelper.FindClassWithEntityName("BotShooterBtr", DnlibHelper.SearchType.Property);
                    var offset = _dumpParser.FindOffsetByTypeName(name, $"-.{fClass.Humanize()}");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ClientShellingController";

                    var fClass = _dnlibHelper.FindClassByTypeName("EFT.GameWorld");
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_ClientShellingController");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    ClientShellingControllerOffset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ClientShellingControllerOffset);
                }

                {
                    entity = "SynchronizableObjectLogicProcessor";

                    var fClass = _dnlibHelper.FindClassWithEntityName("GetAlivePlayerByProfileID", DnlibHelper.SearchType.Method);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_SynchronizableObjectLogicProcessor");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LootList";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "RegisteredPlayers";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MainPlayer";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Grenades";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TransitController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!TransitControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, TransitControllerOffset);
                    goto end;
                }

                {
                    entity = "TransitPoints";

                    var offset = _dumpParser.FindOffsetByTypeName(TransitControllerOffset.Value.TypeName, "System.Collections.Generic.Dictionary<Int32, TransitPoint>");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ClientShellingController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ClientShellingControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ClientShellingControllerOffset);
                    goto end;
                }

                {
                    entity = "ActiveClientProjectiles";

                    var offset = _dumpParser.FindOffsetByName(ClientShellingControllerOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ArtilleryProjectileClient";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "CommonAssets.Scripts.ArtilleryShelling.Client.ArtilleryProjectileClient";

                var ThisClass = _dnlibHelper.FindClassByTypeName(className);

                {
                    entity = "IsActive";

                    var fMethod = _dnlibHelper.FindMethodByName(ThisClass, "Update");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Position";

                    const string searchString = " = Vector3.zero;";
                    var fMethod = _dnlibHelper.FindMethodThatContains(_decompiler_Basic, ThisClass, searchString);
                    var decompiledMethod = _decompiler_Basic.DecompileClassMethod(ThisClass, fMethod.Humanize());
                    var fFieldName = TextHelper.FindSubstringAndGoBackwards(decompiledMethod.Body, searchString, '.');

                    var offset = _dumpParser.FindOffsetByName(className, fFieldName);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> TransitPointOffset = default;

            {
                string name = "TransitPoint";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.TransitPoint";

                {
                    entity = "parameters";

                    TransitPointOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, TransitPointOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TransitParameters";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!TransitPointOffset.Success)
                {
                    nestedStruct.AddOffset(name, TransitPointOffset);
                    goto end;
                }

                {
                    entity = "name";

                    var offset = _dumpParser.FindOffsetByName(TransitPointOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "location";

                    var offset = _dumpParser.FindOffsetByName(TransitPointOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "description";

                    var offset = _dumpParser.FindOffsetByName(TransitPointOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SynchronizableObject";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.SynchronizableObjects.SynchronizableObject";

                {
                    entity = "Type";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SynchronizableObjectLogicProcessor";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "SynchronizableObjects";

                    var fClass = _dnlibHelper.FindClassWithEntityName("GetSyncObjectStrategyByType", DnlibHelper.SearchType.Method);

                    var decompiled = _decompiler_Basic.DecompileClassMethod(fClass, "InitStaticObject");
                    string fField = TextHelper.FindSubstringAndGoBackwards(decompiled.Body, ".Add", '.');

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TripwireSynchronizableObject";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.SynchronizableObjects.TripwireSynchronizableObject";

                {
                    entity = "_tripwireState";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ToPosition>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BtrController";
                SetVariableStatus(name);
                var fClass = _dnlibHelper.FindClassWithEntityName("BotShooterBtr", DnlibHelper.SearchType.Property);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "BtrView";

                    var offset = _dumpParser.FindOffsetByTypeName(fClass.Humanize(), "EFT.Vehicle.BTRView");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            string BtrTurretClassName = default;

            {
                string name = "BTRView";
                SetVariableStatus(name);
                const string className = "EFT.Vehicle.BTRView";

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "turret";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    BtrTurretClassName = offset.Value.TypeName;
                    nestedStruct.AddOffset(entity, offset);
                }
                {
                    entity = "_targetPosition";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BTRTurretView";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "AttachedBot";

                    var offset = _dumpParser.FindOffsetByTypeName(BtrTurretClassName, "System.ValueTuple<ObservedPlayerView, Boolean>");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ExfilController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DumpParser.EntitySearchListEntry> searchEntities = new()
                {
                    new("EFT.Interactive.ExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                    new("EFT.Interactive.ScavExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                };

                string offsetControllerClassName = _dumpParser.FindOffsetGroupWithEntities(searchEntities);

                {
                    entity = "ExfiltrationPointArray";

                    var offset = _dumpParser.FindOffsetByTypeName(offsetControllerClassName, "EFT.Interactive.ExfiltrationPoint[]");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ScavExfiltrationPointArray";

                    var offset = _dumpParser.FindOffsetByTypeName(offsetControllerClassName, "EFT.Interactive.ScavExfiltrationPoint[]");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SecretExfiltrationPointArray";

                    var offset = _dumpParser.FindOffsetByTypeName(offsetControllerClassName, "EFT.Interactive.SecretExfiltrations.SecretExfiltrationPoint[]");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Exfil";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.ExfiltrationPoint";

                {
                    entity = "Settings";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "EligibleEntryPoints";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_status";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ScavExfil";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.ScavExfiltrationPoint";

                {
                    entity = "EligibleIds";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ExfilSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.ExitTriggerSettings";

                {
                    entity = "Name";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Grenade";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Grenade";

                {
                    entity = "IsDestroyed";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName("Throwable");
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "OnDestroy");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Player";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Player";

                {
                    entity = "_characterController";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<MovementContext>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_playerBody";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ProceduralWeaponAnimation>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Corpse";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<Location>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<Profile>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_inventoryController";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_handsController";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ObservedPlayerControllerOffset = default;

            {
                string name = "ObservedPlayerView";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.NextObservedPlayer.ObservedPlayerView";

                {
                    entity = "GroupID";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_GroupId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AccountId";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_AccountId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Voice";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Voice");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlayerBody";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_PlayerBody");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ObservedPlayerController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_ObservedPlayerController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    ObservedPlayerControllerOffset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ObservedPlayerControllerOffset);
                }

                {
                    entity = "Side";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Side");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsAI";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_IsAI");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ObservedPlayerStateContextOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ObservedHandsControllerOffset = default;
            DumpParser.Result<DumpParser.OffsetData> InfoContainerOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ObservedHealthControllerOffset = default;

            {
                string name = "ObservedPlayerController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedPlayerControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedPlayerControllerOffset);
                    goto end;
                }

                string ObservedPlayerControllerTypeName = ObservedPlayerControllerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Player";

                    var offset = _dumpParser.FindOffsetByTypeName(ObservedPlayerControllerTypeName, "EFT.NextObservedPlayer.ObservedPlayerView");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MovementController";

                    DumpParser.Result<DumpParser.OffsetData> offset1 = default;

                    TypeDef foundClass1 = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod1 = _dnlibHelper.FindMethodByName(foundClass1, "get_MovementController");
                    FieldDef fField1 = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod1);
                    offset1 = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField1.GetFieldName());

                    if (offset1.Success)
                    {
                        string typeName2 = offset1.Value.TypeName.Replace("-.", "");
                        TypeDef foundClass2 = _dnlibHelper.FindClassByTypeName(typeName2);
                        MethodDef foundMethod2 = _dnlibHelper.FindMethodByName(foundClass2, "get_ObservedPlayerStateContext");
                        FieldDef fField2 = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod2);
                        ObservedPlayerStateContextOffset = _dumpParser.FindOffsetByName(offset1.Value.TypeName, fField2.GetFieldName());
                    }

                    nestedStruct.AddOffsetChain(entity, new() { offset1, ObservedPlayerStateContextOffset });
                }

                {
                    entity = "HandsController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_HandsController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    ObservedHandsControllerOffset = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ObservedHandsControllerOffset);
                }

                {
                    entity = "HealthController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_HealthController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    ObservedHealthControllerOffset = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ObservedHealthControllerOffset);
                }

                {
                    entity = "InventoryController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_InventoryController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ObservedMovementController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedPlayerStateContextOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedPlayerStateContextOffset);
                    goto end;
                }

                string ObservedPlayerStateContextTypeName = ObservedPlayerStateContextOffset!.Value!.TypeName!.Replace("-.", "");

                {
                    entity = "Rotation";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerStateContextTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Rotation");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedPlayerStateContextTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ObservedHandsController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedHandsControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedHandsControllerOffset);
                    goto end;
                }

                string ObservedHandsControllerTypeName = ObservedHandsControllerOffset!.Value!.TypeName!.Replace("-.", "");

                {
                    entity = "ItemInHands";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedHandsControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_ItemInHands");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedHandsControllerTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ObservedHealthController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedHealthControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedHealthControllerOffset);
                    goto end;
                }

                string ObservedHealthControllerTypeName = ObservedHealthControllerOffset!.Value!.TypeName!.Replace("-.", "");

                {
                    entity = "Player";

                    var offset = _dumpParser.FindOffsetByTypeName(ObservedHealthControllerTypeName, "EFT.NextObservedPlayer.ObservedPlayerView");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlayerCorpse";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedHealthControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_PlayerCorpse");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedHealthControllerTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "HealthStatus";

                    var offset = _dumpParser.FindOffsetByName(ObservedHealthControllerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ProceduralWeaponAnimation";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.ProceduralWeaponAnimation";


                {
                    entity = "_isAiming";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }


                {
                    entity = "_optics";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SightNBone";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "ProceduralWeaponAnimation.SightNBone";

                {
                    entity = "Mod";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ProfileInfoOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileQuestDataOffset = default;
            DumpParser.Result<DumpParser.OffsetData> WishlistManagerOffset = default;

            {
                string name = "Profile";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Profile";

                {
                    entity = "Id";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AccountId";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Info";

                    ProfileInfoOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ProfileInfoOffset);
                }

                {
                    entity = "QuestsData";

                    ProfileQuestDataOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ProfileQuestDataOffset);
                }

                {
                    entity = "WishlistManager";

                    WishlistManagerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, WishlistManagerOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "WishlistManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fType = _dnlibHelper.FindClassByTypeName(WishlistManagerOffset.Value.TypeName.Replace("-.", ""));
                {
                    var fMethod = _dnlibHelper.FindMethodByName(fType, "get_UserItems");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    entity = "Items";

                    var offset = _dumpParser.FindOffsetByName(fType.HumanizeAlt(), fField.HumanizeAlt());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerInfo";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ProfileInfoOffset.Success)
                {
                    nestedStruct.AddOffset(name, ProfileInfoOffset);
                    goto end;
                }

                string ProfileInfoTypeName = ProfileInfoOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "GroupId";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "EntryPoint";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                var registrationDateOffset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, "RegistrationDate");
                {
                    entity = "Side";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity); // MANUAL OFFSET
                    nestedStruct.AddOffset(entity, new(true, new(entity, "[HUMAN] Int32", registrationDateOffset.Value.Offset - 0x4)));
                }

                {
                    entity = "RegistrationDate";
                    nestedStruct.AddOffset(entity, registrationDateOffset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> QuestDataTemplateOffset = default;

            {
                string name = "QuestData";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ProfileQuestDataOffset.Success)
                {
                    nestedStruct.AddOffset(name, ProfileQuestDataOffset);
                    goto end;
                }

                string ProfileQuestDataTypeName = ProfileQuestDataOffset.Value.TypeName.Split('<')[1].TrimEnd('>');

                {
                    entity = "Id";

                    var offset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CompletedConditions";

                    var offset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Template";

                    QuestDataTemplateOffset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, QuestDataTemplateOffset);
                }

                {
                    entity = "Status";

                    var offset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> QuestTemplateOffset = default;

            {
                string name = "QuestTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!QuestDataTemplateOffset.Success)
                {
                    nestedStruct.AddOffset(name, QuestDataTemplateOffset);
                    goto end;
                }

                string QuestDataTemplateTypeName = QuestDataTemplateOffset!.Value!.TypeName!.Replace("-.", "");
                var fClass = _dnlibHelper.FindClassByTypeName(QuestDataTemplateTypeName);

                {
                    entity = "Conditions";

                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Conditions");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    QuestTemplateOffset = _dumpParser.FindOffsetByName(QuestDataTemplateTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, QuestTemplateOffset);
                }

                {
                    entity = "Name";

                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_NameLocaleKey");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(QuestDataTemplateTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionsContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!QuestTemplateOffset.Success)
                {
                    nestedStruct.AddOffset(name, QuestTemplateOffset);
                    goto end;
                }

                string QuestTemplateTypeName = QuestTemplateOffset!.Value!.TypeName!.Replace("-.", "");

                {
                    entity = "ConditionsList";

                    var tClass1 = _dnlibHelper.FindClassByTypeName(QuestTemplateTypeName);
                    var typeSpec = tClass1.BaseType as TypeSpec;
                    var genericInstSig = typeSpec?.TypeSig as GenericInstSig;

                    string dictValueType = genericInstSig?.GenericArguments[1].FullName.Humanize();

                    string tClass2 = _dnlibHelper.FindClassByTypeName(dictValueType).BaseType.FullName.Humanize();
                    string tClass3 = _dnlibHelper.FindClassByTypeName(tClass2).BaseType.FullName.Humanize();

                    var fClass = _dnlibHelper.FindClassByTypeName(tClass3);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Count");

                    string methodBody = _decompiler_Basic.DecompileClassMethod(fClass, "get_Count").Body;
                    string fField = TextHelper.FindSubstringAndGoBackwards(methodBody, ".Count", ' ');

                    var offset = _dumpParser.FindOffsetByName(tClass3, fField);

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestCondition";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.Condition";

                {
                    entity = "<id>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionFindItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionFindItem";

                {
                    entity = "target";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionCounterCreator";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionCounterCreator";

                {
                    entity = "<Conditions>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionVisitPlace";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionVisitPlace";

                {
                    entity = "target";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionPlaceBeacon";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionPlaceBeacon";

                {
                    entity = "zoneId";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ItemHandsController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Item";

                    var fClass = _dnlibHelper.FindClassByTypeName(name);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "GetItem");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName("Player." + name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MovementContext";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.MovementContext";

                {
                    entity = "Player";

                    var offset = _dumpParser.FindOffsetByTypeName(className, "EFT.Player");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_rotation";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InventoryController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Player.PlayerInventoryController";

                {
                    entity = "<Inventory>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> InventoryEquipmentOffset = default;

            {
                string name = "Inventory";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Inventory";

                {
                    entity = "Equipment";

                    InventoryEquipmentOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, InventoryEquipmentOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            string GridsClassName = default;

            {
                string name = "Equipment";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!InventoryEquipmentOffset.Success)
                {
                    nestedStruct.AddOffset(name, InventoryEquipmentOffset);
                    goto end;
                }

                string InventoryEquipmentTypeName = InventoryEquipmentOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Slots";

                    var offset = _dumpParser.FindOffsetByName(InventoryEquipmentTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Slot";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Slot";

                {
                    entity = "<ID>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ContainedItem>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InteractiveLootItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.LootItem";

                {
                    entity = "Item";

                    var fClass = _dnlibHelper.FindClassByTypeName(className);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Item");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "DizSkinningSkeleton";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Diz.Skinning.Skeleton";

                {
                    entity = "_values";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> LootableContainerItemOwnerOffset = default;

            {
                string name = "LootableContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.LootableContainer";

                {
                    entity = "ItemOwner";

                    LootableContainerItemOwnerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, LootableContainerItemOwnerOffset);
                }

                {
                    entity = "<InteractingPlayer>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootableContainerItemOwner";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!LootableContainerItemOwnerOffset.Success)
                {
                    nestedStruct.AddOffset(name, LootableContainerItemOwnerOffset);
                    goto end;
                }

                string LootableContainerItemOwnerTypeName = LootableContainerItemOwnerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "RootItem";

                    var fClass = _dnlibHelper.FindClassByTypeName(LootableContainerItemOwnerTypeName);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_RootItem");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(LootableContainerItemOwnerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Item";

                {
                    entity = "<Template>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }


            {
                string name = "LootItemMod";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Mod";

                {
                    entity = "Slots";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItemWeapon";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Weapon";

                {
                    entity = "<Chambers>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ItemTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.ItemTemplate";

                {
                    entity = "ShortName";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<_id>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "QuestItem";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerBody";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.PlayerBody";

                {
                    entity = "SkeletonRootJoint";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> GetOpticCameraManagerOffset = default;

            {
                string name = "OpticCameraManagerContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                TypeDef fClass = _dnlibHelper.FindClassWithEntityName("get_OpticCameraManager", DnlibHelper.SearchType.Method);

                {
                    entity = "Instance";

                    const string searchMethod = "get_Instance";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "OpticCameraManager";

                    const string searchMethod = "get_OpticCameraManager";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    GetOpticCameraManagerOffset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, GetOpticCameraManagerOffset);
                }

                {
                    entity = "FPSCamera";

                    const string searchMethod = "get_Camera";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "OpticCameraManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!GetOpticCameraManagerOffset.Success)
                {
                    nestedStruct.AddOffset(name, GetOpticCameraManagerOffset);
                    goto end;
                }

                string GetOpticCameraManagerTypeName = GetOpticCameraManagerOffset.Value.TypeName.Replace("-.", "");

                TypeDef fClass = _dnlibHelper.FindClassByTypeName(GetOpticCameraManagerTypeName);

                {
                    entity = "Camera";

                    const string searchMethod = "get_Camera";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CurrentOpticSight";

                    const string searchMethod = "get_CurrentOpticSight";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SightComponent";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                const string className = "EFT.InventoryLogic.SightComponent";
                string entity;

                var skillValueContainerOffset = _dumpParser.FindOffsetByName("EFT.SkillManager", "StrengthBuffJumpHeightInc"); // Moved this to a local
                {
                    entity = "_template";

                    skillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, skillValueContainerOffset);
                }
                {
                    entity = "ScopesSelectedModes";

                    skillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, skillValueContainerOffset);
                }
                {
                    entity = "SelectedScope";

                    skillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, skillValueContainerOffset);
                }
                {
                    entity = "ScopeZoomValue";

                    skillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, skillValueContainerOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SightInterface";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassWithEntityName("CalibrationDistances", DnlibHelper.SearchType.Field);

                {
                    entity = "Zooms";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }
        }

        private void ProcessEnums(StatusContext ctx, StructureGenerator structGenerator)
        {
            void SetVariableStatus(string variable)
            {
                LastStepName = variable;
                ctx.Status(variable);
            }

            {
                const string name = "EPlayerSide";
                const string typeName = "EPlayerSide";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, flags: false);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "ETagStatus";
                const string typeName = "ETagStatus";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, flags: true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EMemberCategory";
                const string typeName = "EMemberCategory";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, flags: true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "WildSpawnType";
                const string typeName = "EFT.WildSpawnType";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EExfiltrationStatus";
                const string typeName = "EFT.Interactive.EExfiltrationStatus";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "SynchronizableObjectType";
                const string typeName = "SynchronizableObjectType";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "ETripwireState";
                const string typeName = "ETripwireState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EQuestStatus";
                const string typeName = "EQuestStatus";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }
        }
    }
}
