﻿using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Strategies
{
    public class StackedImbalances : StrategyBase
    {
        public override string Name { get; set; }
        public override Direction ValidStrategyDirection { get; set; }

        public StackedImbalances(OrderFlowBotState orderFlowBotState, OrderFlowBotDataBars dataBars, string name)
        : base(orderFlowBotState, dataBars, name)
        {
        }

        public override void CheckStrategy()
        {
            if (IsValidLongDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckLong();
            }

            if (IsValidShortDirection() && ValidStrategyDirection == Direction.Flat)
            {
                CheckShort();
            }
        }

        public override void CheckLong()
        {
            if (IsBullishBar() && HasValidAskStackedImbalance() && IsOpenAboveTriggerStrikePrice() && HasValidBidRatio())
            {
                ValidStrategyDirection = Direction.Long;
            }
        }

        public override void CheckShort()
        {
            if (IsBearishBar() && HasValidBidStackedImbalance() && IsOpenBelowTriggerStrikePrice() && HasValidAskRatio())
            {
                ValidStrategyDirection = Direction.Short;
            }
        }

        private bool IsBullishBar()
        {
            return dataBars.Bar.BarType == BarType.Bullish;
        }

        private bool IsBearishBar()
        {
            return dataBars.Bar.BarType == BarType.Bearish;
        }

        private bool HasValidAskStackedImbalance()
        {
            return dataBars.Bar.Imbalances.HasAskStackedImbalances;
        }

        private bool HasValidBidStackedImbalance()
        {
            return dataBars.Bar.Imbalances.HasBidStackedImbalances;
        }

        private bool IsOpenAboveTriggerStrikePrice()
        {
            if (orderFlowBotState.TriggerStrikePrice == 0 || !OrderFlowBotStrategiesProperties.StackedImbalanceValidOpenTSP)
            {
                return true;
            }

            return dataBars.Bar.Prices.Open > orderFlowBotState.TriggerStrikePrice;
        }

        private bool IsOpenBelowTriggerStrikePrice()
        {
            if (orderFlowBotState.TriggerStrikePrice == 0 || !OrderFlowBotStrategiesProperties.StackedImbalanceValidOpenTSP)
            {
                return true;
            }

            return dataBars.Bar.Prices.Open < orderFlowBotState.TriggerStrikePrice;
        }

        private bool HasValidBidRatio()
        {
            if (!OrderFlowBotStrategiesProperties.StackedImbalancesRatiosEnabled)
            {
                return true;
            }

            return dataBars.Bar.Ratios.HasValidBidAbsorptionRatio || dataBars.Bar.Ratios.HasValidBidExhaustionRatio;
        }

        private bool HasValidAskRatio()
        {
            if (!OrderFlowBotStrategiesProperties.StackedImbalancesRatiosEnabled)
            {
                return true;
            }

            return dataBars.Bar.Ratios.HasValidAskAbsorptionRatio || dataBars.Bar.Ratios.HasValidAskExhaustionRatio;
        }
    }
}
