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

                // Class name needs to be determined from dump or hardcoded
                const string className = "BtrController";

                {
                    entity = "BtrView";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BTRView";
                SetVariableStatus(name);
                const string className = "EFT.Vehicle.BTRView";

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "turret";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
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
                const string className = "EFT.Vehicle.BTRTurretView";
                string name = "BTRTurretView";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "_bot";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
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

                {
                    entity = "_playerLookRaycastTransform";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

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
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
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

            {
                string name = "ObservedPlayerController";
                const string typeName = "EFT.NextObservedPlayer.ObservedPlayerController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "InventoryController";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlayerView";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MovementController";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "HealthController";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

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

                const string typeName = "ObservedPlayerHealthController";

                {
                    entity = "_player";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
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

                structGenerator.AddStruct(nestedStruct);
            }

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
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerInfo";
                const string typeName = "EFT.ProfileInfo";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "GroupId";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                var registrationDateOffset = _dumpParser.FindOffsetByName(typeName, "RegistrationDate");
                {
                    entity = "Side";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
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
                    entity = "_player";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
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

            {
                string name = "LootableContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.LootableContainer";

                {
                    entity = "ItemOwner";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ItemController";
                const string typeName = "EFT.InventoryLogic.ItemController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "RootItem";
                    var offset = _dumpParser.FindOffsetByName(typeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

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
