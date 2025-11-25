using Spectre.Console;
using TarkovDumper.Helpers;
using TarkovDumper.UI;

namespace TarkovDumper.Processors
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
        public override void Run(StatusContext ctx)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"Processing {this.GetType()} entries...");
            var structGenerator_offsets = new StructureGenerator("Offsets");
            var structGenerator_enums = new StructureGenerator("Enums");
            ProcessOffsets(ctx, structGenerator_offsets);
            ProcessEnums(ctx, structGenerator_enums);

            AnsiConsole.Clear();

            var sgList = new List<StructureGenerator>()
            {
                structGenerator_offsets,
                structGenerator_enums,
            };
            AnsiConsole.WriteLine(StructureGenerator.GenerateNamespace("SDK", sgList));
            AnsiConsole.WriteLine(StructureGenerator.GenerateReports(sgList));

            string plainSDK = StructureGenerator.GenerateNamespace("SDK", sgList, false);
            File.WriteAllText(Config.OutputPath, plainSDK);
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
                    entity = "LocationId";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BtrController";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SynchronizableObjectLogicProcessor";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LootList";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "RegisteredPlayers";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MainPlayer";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Grenades";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

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

                const string className = "EFT.SynchronizableObjects.SynchronizableObjectLogicProcessor";

                {
                    entity = "_activeSynchronizableObjects";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
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
                    entity = "ToPosition";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BtrController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Class className needs to be determined from dump or hardcoded
                const string className = "BtrController";

                {
                    entity = "BtrView";
                    var offset = _dumpParser.FindOffsetByTypeName(className, "EFT.Vehicle.BTRView");
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
                    entity = "_bot";
                    var offset = _dumpParser.FindOffsetByName(BtrTurretClassName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Throwable";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Throwable";

                {
                    entity = "_isDestroyed";
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

                const string className = "EFT.Player";

                {
                    entity = "MovementContext";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_playerBody";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Corpse";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Location";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Profile";
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
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AccountId";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Voice";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlayerBody";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ObservedPlayerController";
                    ObservedPlayerControllerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ObservedPlayerControllerOffset);
                }

                {
                    entity = "Side";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsAI";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ObservedHealthControllerOffset = default;

            {
                string className = "ObservedPlayerController";
                SetVariableStatus(className);

                StructureGenerator nestedStruct = new(className);

                string entity;

                if (!ObservedPlayerControllerOffset.Success)
                {
                    nestedStruct.AddOffset(className, ObservedPlayerControllerOffset);
                    goto end;
                }

                {
                    entity = "InventoryController";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlayerView";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MovementController";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "HealthController";
                    ObservedHealthControllerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ObservedHealthControllerOffset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InventoryController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.InventoryController";

                {
                    entity = "Inventory";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Inventory";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Inventory";

                {
                    entity = "Equipment";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InventoryEquipment";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.InventoryEquipment";

                {
                    entity = "_cachedSlots";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
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
                    entity = "ID";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                }
                {
                    entity = "ContainedItem";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ObservedPlayerMovementController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.NextObservedPlayer.ObservedPlayerMovementController";

                {
                    entity = "ObservedPlayerStateContext";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }


            {
                string name = "ObservedPlayerStateContext";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.NextObservedPlayer.ObservedPlayerStateContext";

                {
                    entity = "Rotation";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
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

                const string typeName = "ObservedPlayerHealthController";

                {
                    entity = "_player";
                    var offset = _dumpParser.FindOffsetByTypeName(typeName, "EFT.NextObservedPlayer.ObservedPlayerView");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_playerCorpse";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "HealthStatus";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ProfileInfoOffset = default;

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

            end:
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
                string name = "InteractiveLootItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.LootItem";

                {
                    entity = "_item";
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
                    var offset = _dumpParser.FindOffsetByName(LootableContainerItemOwnerTypeName, entity);
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
                    entity = "Template";
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
                    entity = "_id";
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
        }
    }
}
