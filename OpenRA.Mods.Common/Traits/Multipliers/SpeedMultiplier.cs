#region Copyright & License Information
/*
 * Copyright 2007-2016 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

namespace OpenRA.Mods.Common.Traits
{
	[Desc("The speed of this actor is multiplied based on upgrade level if specified.")]
	public class SpeedMultiplierInfo : UpgradeMultiplierTraitInfo
	{
		public override object Create(ActorInitializer init) { return new SpeedMultiplier(this, init.Self.Info.Name); }
	}

	public class SpeedMultiplier : UpgradeMultiplierTrait, ISpeedModifier
	{
		public SpeedMultiplier(SpeedMultiplierInfo info, string actorType)
			: base(info, "SpeedMultiplier", actorType) { }

		int ISpeedModifier.GetSpeedModifier() { return GetModifier(); }
	}
}
