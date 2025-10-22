using dnlib.DotNet;
using Spectre.Console;
using TarkovDumper.Helpers;
using TarkovDumper.UI;

namespace TarkovDumper.Processors
{
    public sealed class ArenaProcessor : AbstractProcessor
    {
        public ArenaConfig Config { get; } = Program.Config.Arena;

        public ArenaProcessor() : base(Program.Config.Arena.AssemblyPath, Program.Config.Arena.DumpPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Config.OutputPath, nameof(Config.OutputPath));
        }

        public override void Run(StatusContext ctx)
        {
            base.Run(ctx);
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
                string entity = "SetLocalProfile";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("StreamerMode");
                nestedStruct.AddString("MethodName", "IsLocalStreamer");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "UpdateTradersSettings";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("InertiaSettings");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "UpdateMergedControls";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("GameSettings");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "AmmoTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "get_LoadUnloadModifier";
                var fClass = _dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method);
                nestedStruct.AddClassName(fClass, "ClassName", entity);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, "MethodName", entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NoMalfunctions";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "GetMalfunctionState";

                var fClass = _dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method);
                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);

                nestedStruct.AddClassName(fClass, "ClassName", entity, true);
                nestedStruct.AddMethodName(fMethod, "GetMalfunctionState", "N/A");

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "get_OpticCameraManager";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("OpticCameraManagerContainer");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ScreenManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "InitChatScreen";

                var fClass = _dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method);
                nestedStruct.AddClassName(fClass, "ClassName", entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("FirearmController");

                var fClass = _dnlibHelper.FindClassByTypeName("FirearmController");

                nestedStruct.AddClassName(fClass, variable, "N/A", true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "EFT.UI.DragAndDrop.GridItemView";
                string entity = "itemComponent.Nickname.SubstringIfNecessary";
                string variable = "MethodName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("GridItemView");

                var fClass = _dnlibHelper.FindClassByTypeName(name);
                var fMethod = _dnlibHelper.FindMethodThatContains(_decompiler_Basic, fClass, entity);

                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "EFT.Animations.ProceduralWeaponAnimation";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("ProceduralWeaponAnimation");

                var fClass = _dnlibHelper.FindClassByTypeName(entity);
                nestedStruct.AddClassName(fClass, variable, entity, true);

                entity = "get_ShotNeedsFovAdjustments";
                variable = "MethodName";
                SetVariableStatus(variable);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "EFT.MovementContext";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("MovementContext");

                var fClass = _dnlibHelper.FindClassByTypeName(entity);
                nestedStruct.AddClassName(fClass, variable, entity, true);

                entity = "SetPhysicalCondition";
                variable = "MethodName";
                SetVariableStatus(variable);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "GrenadeFlashScreenEffect";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new(entity);

                var fClass = _dnlibHelper.FindClassByTypeName(entity);
                nestedStruct.AddClassName(fClass, variable, entity);

                entity = "Update";
                variable = "MethodName";
                SetVariableStatus(variable);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "SetFov";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("FovChanger");

                var fClass = _dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method);
                nestedStruct.AddClassName(fClass, variable, entity);

                variable = "MethodName";
                SetVariableStatus(variable);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "VitalParts";
                string entity = "get_VitalParts";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("VitalParts");

                var fClass = _dnlibHelper.FindClassWithEntityName(name, DnlibHelper.SearchType.Property);

                nestedStruct.AddClassName(fClass, variable, name, true);

                var fMethod_tmp = _decompiler_Basic.DecompileClassMethod(fClass, entity);
                var fCalledMethod = _dnlibHelper.ExtractLinqCalledMethod(fMethod_tmp.Body);

                variable = "MethodName";
                SetVariableStatus(variable);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, fCalledMethod);
                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "EFT.InventoryLogic.Mod";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("InventoryLogic_Mod");

                var fClass = _dnlibHelper.FindClassByTypeName(entity);
                nestedStruct.AddClassName(fClass, variable, entity, true);

                entity = "get_RaidModdable";
                variable = "MethodName";
                SetVariableStatus(variable);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, variable, entity);

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
                string name = "TarkovApplication";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);
                const string ClassName = "EFT.TarkovApplication";
                string entity = "GameOperationSubclass";

                {
                    var fClass = _dnlibHelper.FindClassWithEntityName("IsInSession", DnlibHelper.SearchType.Property);
                    var offset = _dumpParser.FindOffsetByTypeName(ClassName, $"-.{fClass.Humanize()}");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
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
                    entity = "LocationId";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName("EFT.GameWorld");
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_LocationId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

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

                {
                    entity = "IsInRaid";

                    var offset = _dumpParser.FindOffsetByName(name, "SpeedLimitsEnabled"); // MANUAL OFFSET
                    nestedStruct.AddOffset(entity, new(offset.Success, new(entity, "[HUMAN] Bool", offset.Value.Offset + 0x8)));
                }

                {
                    entity = "LoadBundlesAndCreatePools";

                    const string searchString = " = loadBundlesAndCreatePools;";
                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName("EFT.GameWorld");
                    MethodDef foundMethod = _dnlibHelper.FindMethodThatContains(_decompiler_Async, foundClass, searchString);
                    var decompiled = _decompiler_Async.DecompileClassMethod(foundClass, foundMethod.Humanize());

                    var startIndex = decompiled.Body.IndexOf("= loadBundlesAndCreatePools");
                    string fieldName = TextHelper.FindSubstringAndGoBackwards(decompiled.Body, searchString);

                    var offset = _dumpParser.FindOffsetByName(name, fieldName);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "WorldController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                List<DumpParser.EntitySearchListEntry> searchEntities = new()
                {
                    new("EFT.Interactive.WorldInteractiveObject[]", DumpParser.SearchType.TypeName),
                };

                string offsetControllerClassName = _dumpParser.FindOffsetGroupWithEntities(searchEntities);

                {
                    entity = "Interactables";

                    var offset = _dumpParser.FindOffsetByTypeName(offsetControllerClassName, "EFT.Interactive.WorldInteractiveObject[]");
                    nestedStruct.AddOffset(entity, offset);
                }
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Interactable";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.WorldInteractiveObject";

                {
                    entity = "Id";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "KeyId";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_doorState";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LevelSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "AmbientMode";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "EquatorColor";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "GroundColor";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "EFTHardSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "DecelerationSpeed";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LOOT_RAYCAST_DISTANCE";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "DOOR_RAYCAST_DISTANCE";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "STOP_AIMING_AT";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MOUSE_LOOK_HORIZONTAL_LIMIT";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MOUSE_LOOK_VERTICAL_LIMIT";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AIR_CONTROL_SAME_DIR";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AIR_CONTROL_NONE_OR_ORT_DIR";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "WEAPON_OCCLUSION_LAYERS";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "POSE_CHANGING_SPEED";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MED_EFFECT_USING_PANEL";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GlobalConfigs";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Inertia";

                    TypeDef fClass = _dnlibHelper.FindClassWithEntityName("UpdateTradersSettings", DnlibHelper.SearchType.Method);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InertiaSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "Inertia";

                TypeDef fClass = _dnlibHelper.FindClassWithEntityName("UpdateTradersSettings", DnlibHelper.SearchType.Method);
                var fOffset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                if (!fOffset.Success)
                {
                    nestedStruct.AddOffset(entity, fOffset);
                    goto end;
                }

                {
                    entity = "FallThreshold";

                    var offset = _dumpParser.FindOffsetByName(fOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BaseJumpPenaltyDuration";

                    var offset = _dumpParser.FindOffsetByName(fOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BaseJumpPenalty";

                    var offset = _dumpParser.FindOffsetByName(fOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MoveTimeRange";

                    var offset = _dumpParser.FindOffsetByName(fOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }


            {
                string name = "InventoryBlur";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;
                {
                    entity = "enabled";

                    var offset = _dumpParser.FindOffsetByTypeName(name, "Boolean");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_upsampleTexDimension";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_blurCount";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NightVision";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;
                const string className = "BSG.CameraEffects.NightVision";

                {
                    entity = "_on";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ThermalVision";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;
                const string className = "ThermalVision";

                {
                    entity = "Material";

                    var offset = _dumpParser.FindOffsetByTypeName(className, $"UnityEngine.Material");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "On";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsNoisy";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsFpsStuck";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsMotionBlurred";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsGlitch";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsPixelated";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ChromaticAberrationThermalShift";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "UnsharpRadiusBlur";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "UnsharpBias";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
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

                {
                    entity = "WeaponSource";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_WeaponSource");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Renderers";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Player";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;
                string BodyAnimatorClassName = default;

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
                    entity = "_animators";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    BodyAnimatorClassName = offset.Value.TypeName;
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Corpse";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }


                {
                    entity = "<Profile>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Physical";

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

                {
                    entity = "EnabledAnimators";

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
                    entity = "NickName";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_NickName");
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

                {
                    entity = "VisibleToCameraType";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_VisibleToCameraType");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ObservedPlayerStateContextOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ObservedHandsControllerOffset = default;
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

                string ObservedPlayerStateContextTypeName = ObservedPlayerStateContextOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Rotation";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerStateContextTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Rotation");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedPlayerStateContextTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Velocity";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerStateContextTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Velocity");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedPlayerStateContextTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> BundleAnimationBonesOffset = default;

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

                string ObservedHandsControllerTypeName = ObservedHandsControllerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "ItemInHands";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedHandsControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_ItemInHands");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedHandsControllerTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    var foundClass = _dnlibHelper.FindClassByTypeName(ObservedHandsControllerTypeName);
                    var getBab = _dnlibHelper.FindMethodByName(foundClass, "get_BundleAnimationBones");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(getBab);
                    BundleAnimationBonesOffset = _dumpParser.FindOffsetByName(ObservedHandsControllerTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset("BundleAnimationBones", BundleAnimationBonesOffset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ProceduralWeaponAnimationOffset = default;

            {
                string name = "BundleAnimationBonesController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                if (!BundleAnimationBonesOffset.Success)
                {
                    nestedStruct.AddOffset(name, BundleAnimationBonesOffset);
                    goto end;
                }
                string typeName = BundleAnimationBonesOffset.Value.TypeName.Replace("-.", "");
                {
                    var foundClass = _dnlibHelper.FindClassByTypeName(typeName);
                    var getBab = _dnlibHelper.FindMethodByName(foundClass, "get_ProceduralWeaponAnimation");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(getBab);
                    ProceduralWeaponAnimationOffset = _dumpParser.FindOffsetByName(typeName, fField.GetFieldName());

                    nestedStruct.AddOffset("ProceduralWeaponAnimationObs", ProceduralWeaponAnimationOffset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ProceduralWeaponAnimationObs";
                const string className = "EFT.Animations.ProceduralWeaponAnimation";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                var aimingOffset = _dumpParser.FindOffsetByName(className, "_isAiming");
                if (aimingOffset.Success)
                {
                    nestedStruct.AddOffset("_isAimingObs", aimingOffset);
                }

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

                string ObservedHealthControllerTypeName = ObservedHealthControllerOffset.Value.TypeName.Replace("-.", "");

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
                string name = "SimpleCharacterController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "_collisionMask";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_speedLimit";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_sqrSpeedLimit";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "velocity";

                    TypeDef fClass = _dnlibHelper.FindClassByTypeName(name);
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, "get_velocity");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> PhysicalValueOffset = default;

            {
                string name = "Physical";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                TypeDef fClass = _dnlibHelper.FindClassWithEntityName("SubscribeToAudibleEffects", DnlibHelper.SearchType.Method);

                {
                    entity = "Stamina";

                    PhysicalValueOffset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, PhysicalValueOffset);
                }

                {
                    entity = "HandsStamina";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Oxygen";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Overweight";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "WalkOverweight";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "WalkSpeedLimit";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Inertia";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "WalkOverweightLimits";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BaseOverweightLimits";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SprintOverweightLimits";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SprintWeightFactor";

                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, "get_SprintSpeed");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod, 2);

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SprintAcceleration";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PreSprintAcceleration";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsOverweightA";

                    const string substring = "&& weight > PreviousWeight";
                    MethodDef fMethod = _dnlibHelper.FindMethodThatContains(_decompiler_Basic, fClass, substring);
                    var decompiled = _decompiler_Basic.DecompileClassMethod(fClass, fMethod.Humanize());
                    string fieldName = TextHelper.FindSubstringAndGoBackwards(decompiled.Body, substring, '(');

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fieldName);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsOverweightB";

                    const string substring = "!= flag2)";
                    MethodDef fMethod = _dnlibHelper.FindMethodThatContains(_decompiler_Basic, fClass, substring);
                    var decompiled = _decompiler_Basic.DecompileClassMethod(fClass, fMethod.Humanize());
                    string fieldName = TextHelper.FindSubstringAndGoBackwards(decompiled.Body, substring, '(');

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fieldName);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PhysicalValue";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!PhysicalValueOffset.Success)
                {
                    nestedStruct.AddOffset(name, PhysicalValueOffset);
                    goto end;
                }

                string PhysicalValueTypeName = PhysicalValueOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Current";

                    var offset = _dumpParser.FindOffsetByName(PhysicalValueTypeName, entity);
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
                    entity = "HandsContainer";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_shotDirection";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Breath";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Shootingg";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Mask";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_isAiming";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_aimingSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_fovCompensatoryDistance";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_compensatoryScale";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CameraSmoothOut";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PositionZeroSum";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ShotNeedsFovAdjustments>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_optics";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MotionReact";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "HandsContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.PlayerSpring";

                {
                    entity = "CameraOffset";

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

            {
                string name = "MotionEffector";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "_mouseProcessors";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_movementProcessors";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BreathEffector";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.BreathEffector";

                {
                    entity = "Intensity";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ShotEffector";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "NewShotRecoil";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NewShotRecoil";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.NewRecoil.NewRecoilShotEffect";

                {
                    entity = "IntensitySeparateFactors";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "VisorEffect";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Intensity";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ProfileInfoOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileTaskConditionCountersOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileQuestDataOffset = default;

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

                structGenerator.AddStruct(nestedStruct);
            }

            /// TODO: DISABLED
            //{
            //    string name = "OverallCounters";
            //    SetVariableStatus(name);

            //    StructureGenerator nestedStruct = new(name);

            //    string entity;

            //    if (!OverallCountersOffset.Success)
            //    {
            //        nestedStruct.AddOffset(name, OverallCountersOffset);
            //        goto end;
            //    }

            //    string OverallCountersTypeName = OverallCountersOffset.Value.TypeName.Replace("-.", "");

            //    {
            //        entity = "Counters";

            //        var offset = _dumpParser.FindOffsetByName(OverallCountersTypeName, entity);
            //        nestedStruct.AddOffset(entity, offset);
            //    }

            //end:
            //    structGenerator.AddStruct(nestedStruct);
            //}

            DumpParser.Result<DumpParser.OffsetData> PlayerInfoSettingsOffset = default;

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
                    entity = "Nickname";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Settings";

                    var settingsClass = _dnlibHelper.FindClassWithEntityName("StandingForKill", DnlibHelper.SearchType.Field);
                    PlayerInfoSettingsOffset = _dumpParser.FindOffsetByTypeName(ProfileInfoTypeName, $"-.{settingsClass.Humanize()}");
                    nestedStruct.AddOffset(entity, PlayerInfoSettingsOffset);
                }

                var registrationDateOffset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, "RegistrationDate");
                {
                    entity = "Side";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, new(true, new(entity, "[HUMAN] Int32", registrationDateOffset.Value.Offset - 0x4)));
                }

                {
                    entity = "RegistrationDate";
                    nestedStruct.AddOffset(entity, registrationDateOffset);
                }

                {
                    entity = "MemberCategory";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Experience";

                    var fClass = _dnlibHelper.FindClassByTypeName(ProfileInfoTypeName);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Experience");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerInfoSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!PlayerInfoSettingsOffset.Success)
                {
                    nestedStruct.AddOffset(name, PlayerInfoSettingsOffset);
                    goto end;
                }

                string PlayerInfoSettingsTypeName = PlayerInfoSettingsOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Role";

                    var offset = _dumpParser.FindOffsetByName(PlayerInfoSettingsTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> SkillValueContainerOffset = default;

            {
                string name = "SkillManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.SkillManager";

                {
                    entity = "StrengthBuffJumpHeightInc";

                    SkillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, SkillValueContainerOffset);
                }

                {
                    entity = "StrengthBuffThrowDistanceInc";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MagDrillsLoadSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MagDrillsUnloadSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SkillValueContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!SkillValueContainerOffset.Success)
                {
                    nestedStruct.AddOffset(name, SkillValueContainerOffset);
                    goto end;
                }

                string SkillValueContainerTypeName = SkillValueContainerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Value";

                    var offset = _dumpParser.FindOffsetByName(SkillValueContainerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
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
                string name = "FirearmController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Player.FirearmController";

                {
                    entity = "Fireport";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "TotalCenterOfImpact";

                    var fClass = _dnlibHelper.FindClassByTypeName(className.Split('.')[1]);
                    var methodBody = _decompiler_Basic.DecompileClassMethod(fClass, "WeaponModified").Body;
                    var fField = TextHelper.FindSubstringAndGoBackwards(methodBody, " = Item.GetTotalCenterOfImpact", '.');

                    var offset = _dumpParser.FindOffsetByName(className, fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ClientFirearmController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.ClientFirearmController";

                {
                    entity = "WeaponLn";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ShotIndex";

                    var fClass = _dnlibHelper.FindClassByTypeName(className);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "DryShot");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
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
                    entity = "<CurrentState>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_states";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_tilt";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_rotation";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_physicalCondition";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_speedLimitIsDirty";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<StateSpeedLimit>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<StateSprintSpeedLimit>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_lookDirection";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<WalkInertia>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<SprintBrakeInertia>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_movementStates";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MovementState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.MovementState";

                {
                    entity = "Name";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AnimatorStateHash";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "StickToGround";

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
                    entity = "Grids";

                    var offset = _dumpParser.FindOffsetByName(InventoryEquipmentTypeName, entity);
                    GridsClassName = offset.Value.TypeName.Replace("-.", "").Replace("[]", "");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Slots";

                    var offset = _dumpParser.FindOffsetByName(InventoryEquipmentTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            string GridContainedItemsClassName = default;

            {
                string name = "Grids";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassByTypeName(GridsClassName);

                {
                    var fFMethod = _dnlibHelper.FindMethodByName(fClass, "get_ItemCollection");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fFMethod);
                    GridContainedItemsClassName = fField.GetTypeName();
                    entity = "ContainedItems";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.Humanize());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GridContainedItems";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;
                {
                    entity = "Items";

                    var offset = _dumpParser.FindOffsetByTypeName(GridContainedItemsClassName, "System.Collections.Generic.List<Item>");
                    nestedStruct.AddOffset(entity, offset);
                }

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
                string name = "InteractiveCorpse";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.Corpse";

                {
                    entity = "PlayerBody";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
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

                {
                    entity = "Template";

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

                {
                    entity = "StackObjectsCount";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Version";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> LootItemModGridsOffset = default;

            {
                string name = "LootItemMod";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Mod";

                {
                    entity = "Grids";

                    LootItemModGridsOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, LootItemModGridsOffset);
                }

                {
                    entity = "Slots";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> LootItemModGridsItemCollectionOffset = default;

            {
                string name = "LootItemModGrids";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!LootItemModGridsOffset.Success)
                {
                    nestedStruct.AddOffset(name, LootItemModGridsOffset);
                    goto end;
                }

                string LootItemModGridsTypeName = LootItemModGridsOffset.Value.TypeName.Replace("-.", "").Split('[')[0];

                {
                    entity = "ItemCollection";

                    var fClass = _dnlibHelper.FindClassByTypeName(LootItemModGridsTypeName);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_ItemCollection");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    LootItemModGridsItemCollectionOffset = _dumpParser.FindOffsetByName(LootItemModGridsTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, LootItemModGridsItemCollectionOffset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItemModGridsItemCollection";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!LootItemModGridsItemCollectionOffset.Success)
                {
                    nestedStruct.AddOffset(name, LootItemModGridsItemCollectionOffset);
                    goto end;
                }

                string LootItemModGridsItemCollectionTypeName = LootItemModGridsItemCollectionOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "List";

                    var offset = _dumpParser.FindOffsetByTypeName(LootItemModGridsItemCollectionTypeName, "System.Collections.Generic.List<Item>");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
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

                {
                    entity = "_magSlotCache";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "FireMode";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "FireModeComponent";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.FireModeComponent";

                {
                    entity = "FireMode";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItemMagazine";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("GetAmmoCountByLevel", DnlibHelper.SearchType.Method),
                    new("VisibleAmmoRanges", DnlibHelper.SearchType.Property),
                };
                var magazineClassA = _dnlibHelper.FindClassWithEntities(searchEntities);
                var magazineClassB = _dnlibHelper.FindClassWithEntityName("LoadUnloadModifier", DnlibHelper.SearchType.Field);

                {
                    entity = "Cartridges";

                    var fMethod = _dnlibHelper.FindMethodByName(magazineClassA, "get_Cartridges");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(magazineClassA.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LoadUnloadModifier";

                    var offset = _dumpParser.FindOffsetByName(magazineClassB.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MagazineClass";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var magazineClass = _dnlibHelper.FindClassWithEntityName("GetMaxVisibleAmmo", DnlibHelper.SearchType.Method);

                {
                    entity = "StackObjectsCount";

                    var offset = _dumpParser.FindOffsetByName(magazineClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "StackSlot";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.StackSlot";

                {
                    entity = "_items";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MaxCount";

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

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ModTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.ModTemplate";

                {
                    entity = "Velocity";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "AmmoTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.AmmoTemplate";

                {
                    entity = "InitialSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BallisticCoeficient";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BulletMassGram";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BulletDiameterMilimeters";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "WeaponTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.WeaponTemplate";

                {
                    entity = "Velocity";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> PlayerBodySlotViewsOffset = default;
            DumpParser.Result<DumpParser.OffsetData> PointOfViewOffset = default;

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

                {
                    entity = "BodySkins";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_bodyRenderers";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SlotViews";

                    PlayerBodySlotViewsOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, PlayerBodySlotViewsOffset);
                }

                {
                    entity = "PointOfView";

                    PointOfViewOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, PointOfViewOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PointOfView";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!PointOfViewOffset.Success)
                {
                    nestedStruct.AddOffset(name, PointOfViewOffset);
                    goto end;
                }

                string PointOfViewTypeName = PointOfViewOffset.Value.TypeName.Replace("-.", "").Split('<')[0];

                {
                    entity = "POV";

                    var offset = _dumpParser.FindOffsetByTypeName(PointOfViewTypeName, "Var");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerSpring";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.PlayerSpring";

                {
                    entity = "CameraTransform";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerBodySubclass";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassWithEntityName("DestroyCurrentModel", DnlibHelper.SearchType.Method);

                {
                    entity = "Dresses";

                    string fixedClassName = fClass.Humanize(true).Split("EFT.")[1].Split('+')[0] + '.' + fClass.Humanize();

                    var offset = _dumpParser.FindOffsetByName(fixedClassName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Dress";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Visual.Dress";

                {
                    entity = "Renderers";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Skeleton";
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

            {
                string name = "LoddedSkin";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Visual.LoddedSkin";

                {
                    entity = "_lods";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Skin";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Diz.Skinning.Skin";

                {
                    entity = "_skinnedMeshRenderer";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TorsoSkin";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Visual.TorsoSkin";

                {
                    entity = "_skin";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SlotViewsContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!PlayerBodySlotViewsOffset.Success)
                {
                    nestedStruct.AddOffset(name, PlayerBodySlotViewsOffset);
                    goto end;
                }

                string PlayerBodySlotViewsTypeName = PlayerBodySlotViewsOffset.Value.TypeName.Replace("-.", "").Split('<')[0];

                {
                    entity = "Dict";

                    var offset = _dumpParser.FindOffsetByTypeName(PlayerBodySlotViewsTypeName, "System.Collections.Generic.Dictionary<Var, Var>");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "WeatherController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Weather.WeatherController";

                {
                    entity = "WeatherDebug";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "WeatherDebug";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Weather.WeatherDebug";

                {
                    entity = "isEnabled";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "WindMagnitude";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CloudDensity";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Fog";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Rain";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LightningThunderProbability";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TOD_Scattering";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "sky";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TOD_Sky";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "<Cycle>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset("Cycle", offset);
                }

                {
                    entity = "TOD_Components";

                    var offset = _dumpParser.FindOffsetByTypeName(name, "-.TOD_Components");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TOD_CycleParameters";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Hour";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TOD_Components";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "TOD_Time";

                    var offset = _dumpParser.FindOffsetByTypeName(name, "-.TOD_Time");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TOD_Time";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "LockCurrentTime";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }
            {
                string name = "PrismEffects";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "useVignette";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "useExposure";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "CC_Vintage";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "amount";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> GPUInstancerManagerRuntimeDataListOffset = default;

            {
                string name = "GPUInstancerManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "GPUInstancer.GPUInstancerManager";

                {
                    entity = "runtimeDataList";

                    GPUInstancerManagerRuntimeDataListOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, GPUInstancerManagerRuntimeDataListOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "RuntimeDataList";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!GPUInstancerManagerRuntimeDataListOffset.Success)
                {
                    nestedStruct.AddOffset(name, GPUInstancerManagerRuntimeDataListOffset);
                    goto end;
                }

                string GPUInstancerManagerRuntimeDataListTypeName = GPUInstancerManagerRuntimeDataListOffset.Value.TypeName.Split('<')[1].TrimEnd('>');

                {
                    entity = "instanceBounds";

                    var offset = _dumpParser.FindOffsetByName(GPUInstancerManagerRuntimeDataListTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> GameSettingsInnerContainerOffset = default;

            {
                string name = "GameSettingsContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("Game", DnlibHelper.SearchType.Field),
                    new("Sound", DnlibHelper.SearchType.Field),
                    new("PostFx", DnlibHelper.SearchType.Field),
                    new("Graphics", DnlibHelper.SearchType.Field),
                    new("Control", DnlibHelper.SearchType.Field),
                };
                var gameSettingsContainerClass = _dnlibHelper.FindClassWithEntities(searchEntities);

                {
                    entity = "Game";

                    var offset = _dumpParser.FindOffsetByName(gameSettingsContainerClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Graphics";

                    GameSettingsInnerContainerOffset = _dumpParser.FindOffsetByName(gameSettingsContainerClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, GameSettingsInnerContainerOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GameSettingsInnerContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!GameSettingsInnerContainerOffset.Success)
                {
                    nestedStruct.AddOffset(name, GameSettingsInnerContainerOffset);
                    goto end;
                }

                string GameSettingsInnerContainerTypeName = GameSettingsInnerContainerOffset.Value.TypeName.Replace("-.", "").Split('<')[0];

                {
                    entity = "Settings";

                    var offset = _dumpParser.FindOffsetByName(GameSettingsInnerContainerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Controller";

                    var offset = _dumpParser.FindOffsetByName(GameSettingsInnerContainerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GameSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("FieldOfView", DnlibHelper.SearchType.Field),
                    new("HeadBobbing", DnlibHelper.SearchType.Field),
                    new("AutoEmptyWorkingSet", DnlibHelper.SearchType.Field),
                };
                var gameSettingsClass = _dnlibHelper.FindClassWithEntities(searchEntities);

                {
                    entity = "FieldOfView";

                    var offset = _dumpParser.FindOffsetByName(gameSettingsClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "HeadBobbing";

                    var offset = _dumpParser.FindOffsetByName(gameSettingsClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AutoEmptyWorkingSet";

                    var offset = _dumpParser.FindOffsetByName(gameSettingsClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GraphicsSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!GameSettingsInnerContainerOffset.Success)
                {
                    nestedStruct.AddOffset(name, GameSettingsInnerContainerOffset);
                    goto end;
                }

                string GraphicsSettingsInnerContainerTypeName = GameSettingsInnerContainerOffset.Value.TypeName.Split('<')[1].Split(',')[0];

                {
                    entity = "DisplaySettings";

                    var offset = _dumpParser.FindOffsetByName(GraphicsSettingsInnerContainerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NetworkContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var offsetGroup = _dumpParser.FindOffsetGroupWithEntities(new() { new("NextRequestIndex", DumpParser.SearchType.OffsetName) });

                {
                    entity = "NextRequestIndex";

                    var offset = _dumpParser.FindOffsetByName(offsetGroup, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PhpSessionId";

                    var fClass = _dnlibHelper.FindClassByTypeName(offsetGroup.Replace("-.", ""));
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_PhpSessionId");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(offsetGroup, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AppVersion";

                    var fClass = _dnlibHelper.FindClassByTypeName(offsetGroup.Replace("-.", ""));
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_AppVersion");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(offsetGroup, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ScreenManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("InitChatScreen", DnlibHelper.SearchType.Method),
                };
                var screenManagerClass = _dnlibHelper.FindClassWithEntities(searchEntities);

                {
                    entity = "Instance";

                    var fMethod = _dnlibHelper.FindMethodByName(screenManagerClass, "get_Instance");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(screenManagerClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CurrentScreenController";

                    var baseType = screenManagerClass.BaseType.FullName.Humanize();

                    var fClass = _dnlibHelper.FindClassByTypeName(baseType);
                    string decompiledMethod = _decompiler_Basic.DecompileClassMethod(fClass, "get_CurrentScreenController").Body;
                    string fField = TextHelper.FindSubstringAndGoBackwards(decompiledMethod, ";");

                    var offset = _dumpParser.FindOffsetByName(baseType, fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "CurrentScreenController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("Closed", DnlibHelper.SearchType.Property),
                    new("CloseScreenForced", DnlibHelper.SearchType.Method),
                };
                var currentScreenControllerClass = _dnlibHelper.FindClassWithEntities(searchEntities);

                {
                    entity = "Generic";

                    string decompiledMethod = _decompiler_Basic.DecompileClassMethod(currentScreenControllerClass, "DisplayScreen").Body;
                    string fField = TextHelper.FindSubstringAndGoBackwards(decompiledMethod, ".Show(", '.');
                    string typeName = currentScreenControllerClass.DeclaringType.Humanize() + '.' + currentScreenControllerClass.Humanize();

                    var offset = _dumpParser.FindOffsetByName(typeName, fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }
            {
                string name = "BSGGameSetting";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "ValueClass";

                    nestedStruct.AddOffset(entity, new(true, new(entity, "[HUMAN] ulong", 0x28)));
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BSGGameSettingValueClass";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Value";

                    nestedStruct.AddOffset(entity, new(true, new(entity, "[HUMAN] T", 0x30)));
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SSAA";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "OpticMaskMaterial";

                    nestedStruct.AddOffset(entity, new(true, new(entity, "[HUMAN] UnityEngine.Material", 0x58)));
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BloomAndFlares";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "BloomIntensity";

                    nestedStruct.AddOffset(entity, new(true, new(entity, "[HUMAN] Single", 0xB8)));
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
                string name = "OpticSight";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.CameraControl.OpticSight";

                {
                    entity = "LensRenderer";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SightComponent";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                const string className = "EFT.InventoryLogic.SightComponent";
                string entity;

                {
                    entity = "_template";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ScopesSelectedModes";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SelectedScope";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ScopeZoomValue";

                    SkillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, SkillValueContainerOffset);
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

            var NetworkGameData = _dnlibHelper.FindClassWithEntityName("IsAppropriateStatusChange", DnlibHelper.SearchType.Method);
            {
                string name = "NetworkGame";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                const string className = "EFT.CommonNetworkGame`3";
                string entity;

                {
                    entity = "NetworkGameData";
                    var offset = _dumpParser.FindOffsetByTypeName(className, "-." + NetworkGameData.Humanize());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NetworkGameData";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "raidMode";
                    var offset = _dumpParser.FindOffsetByName(NetworkGameData.Humanize(), entity);
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
                const string name = "EPlayerState";
                const string typeName = "EPlayerState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

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
                const string name = "ERaidMode";
                const string typeName = "EFT.ERaidMode";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EMalfunctionState";
                const string typeName = "EMalfunctionState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EPhysicalCondition";
                const string typeName = "EFT.EPhysicalCondition";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EProceduralAnimationMask";
                const string typeName = "EFT.Animations.EProceduralAnimationMask";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, true);

                structGenerator.AddStruct(nestedStruct);
            }


            {
                const string name = "EFireMode";
                const string typeName = "EFireMode";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "ColorType";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new("ArmbandColorType", StructureGenerator.eStructureType.Enum);

                var fClass = _dnlibHelper.FindClassWithEntityName("GetTeamDarkColor", DnlibHelper.SearchType.Method);
                var eType = _dnlibHelper.FindEnumByTypeName(fClass.FullName + "." + name);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "ECameraType";
                const string typeName = "EFT.CameraControl.ECameraType";
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