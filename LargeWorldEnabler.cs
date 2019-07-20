using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;

namespace LargeWorldEnabler
{
	public class LargeWorldEnabler : Mod
	{
		FieldInfo WorldGen_lastMaxTilesX;
		FieldInfo WorldGen_lastMaxTilesY;

		public override void Load() {
			//if (ModLoader.version < new Version(0, 10))
			//{
			//	throw new Exception("\nThis mod uses functionality only present in the latest tModLoader versions. Please update tModLoader to use this mod\n\n");
			//}

			On.Terraria.WorldGen.clearWorld += WorldGen_clearWorld;

			WorldGen_lastMaxTilesX = typeof(WorldGen).GetField("lastMaxTilesX", BindingFlags.Static | BindingFlags.NonPublic);
			WorldGen_lastMaxTilesY = typeof(WorldGen).GetField("lastMaxTilesY", BindingFlags.Static | BindingFlags.NonPublic);
		}

		private void WorldGen_clearWorld(On.Terraria.WorldGen.orig_clearWorld orig) {
			int lastMaxTilesX = (int)WorldGen_lastMaxTilesX.GetValue(null);
			int lastMaxTilesY = (int)WorldGen_lastMaxTilesY.GetValue(null);

			// TODO: investigate cpu/ram trade-off for reducing this later when regular-sized worlds loaded.
			if (Main.maxTilesX > 8400 && Main.maxTilesX > lastMaxTilesX || Main.maxTilesY > 2400 && Main.maxTilesY > lastMaxTilesY) {
				// Goal: Increase limits, don't decrease anything lower than normal max for compatibility.

				// TODO: dynamically change mapTargetX and Y to support any dimensions. (simple division.)
				// Map render targets. -- ingame map number of images to write to. The textures themselves
				Main.mapTargetX = 10; // change that 4 in vanilla to target-x
				Main.mapTargetY = 4; // change that 
				Main.instance.mapTarget = new RenderTarget2D[Main.mapTargetX, Main.mapTargetY];

				int intendedMaxX = Math.Max(Main.maxTilesX + 1, 8401);
				int intendedMaxY = Math.Max(Main.maxTilesY + 1, 2401);

				// Individual map tiles
				Main.Map = new WorldMap(intendedMaxX, intendedMaxY);

				// Space for more tiles -- Actual tiles
				Main.tile = new Tile[intendedMaxX, intendedMaxY];
				// Color for each tile

				Main.initMap = new bool[Main.mapTargetX, Main.mapTargetY];
				Main.mapWasContentLost = new bool[Main.mapTargetX, Main.mapTargetY];
			}
			orig();
			
			//8400 x 2400 -- Actual dimensions of tile array
			//Main.maxTilesX = 16800;
			//Main.maxTilesY = 4800;

			// Initialized later, not needed.
			//RemoteClient.TileSections = new bool[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];
			//for (int i = 0; i < 256; i++)
			//{
			//	Netplay.Clients[i] = new RemoteClient();
			//	Netplay.Clients[i].Reset();
			//	Netplay.Clients[i].Id = i;
			//	Netplay.Clients[i].ReadBuffer = new byte[1024];
			//}
		}
	}
}
