using System;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public static unsafe class Update {
        public static (GameModel, L<Cmd<Msg>>) UpdateGame(Msg msg, GameModel model) {
            switch (msg.Kind) {
                case MsgKind.Tick:
                    return GameLogic.UpdateTick(model, msg.DeltaTime);
                case MsgKind.SlotClicked:
                    return RerollSlot(model, msg.Slot);
                case MsgKind.Restart:
                    return (Utils.NewModel, new());
                case MsgKind.UnitClicked:
                    return (new GameModel(model) { SelectedUnit = msg.Id }, new());
            }
            throw new NotImplementedException("Message is not handled");
        }

        private static (GameModel, L<Cmd<Msg>>) RerollSlot(GameModel model, SlotKind slotKind) {
            static bool SlotOfKind(Slot slot, SlotKind* kind) {
                return slot.Item.SlotKind == *kind;
            }

            if (
                model.Players.FindIndex(&Utils.PlayerIsHuman).Test(out var playerIndex)
                && model.Players.At(playerIndex).Test(out var player)) {
                if (
                    (player.Coins > 0)
                    && player.Slots
                            .FindIndex(Cf.New<Slot, SlotKind, bool>(&SlotOfKind, slotKind))
                            .Test(out var slotIndex)
                    && player.Slots.At(slotIndex).Test(out var slot)
                ) {
                    var items = Utils.ShuffleItems(model.Items);
                    if (Utils.FirstItemWithSlot(items, slotKind).Test(out var item)) {
                        slot.PrevItem = Maybe<Item>.Just(slot.Item);
                        slot.SwapProgress = 0;
                        slot.Item = item;
                        player.Slots = player.Slots.Replace(slotIndex, slot);
                        player.Coins--;
                        model.Players = model.Players.Replace(playerIndex, player);
                    }
                }
            }

            return (model, new());
        }
    }
}