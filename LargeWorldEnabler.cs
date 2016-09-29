using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;

namespace LargeWorldEnabler
{
	public class LargeWorldEnabler : Mod
	{
		public LargeWorldEnabler()
		{
			Properties = new ModProperties();
		}

		public override void Load()
		{
			// Older versions don't have the correct variables.
			if (ModLoader.version < new Version(0, 8, 3, 2))
			{
				throw new Exception("\nThis mod uses functionality only present in the latest tModLoader versions. Please update tModLoader to use this mod\n\n");
			}

			//8400 x 2400 -- Actual dimensions of tile array
			Main.maxTilesX = 16800;
			Main.maxTilesY = 4800;

			// Map render targets. -- ingame map number of images to write to. The textures themselves
			Main.mapTargetX = 10; // change that 4 in vanilla to target-x
			Main.mapTargetY = 4; // change that 
			Main.instance.mapTarget = new RenderTarget2D[Main.mapTargetX, Main.mapTargetY];

			// Individual map tiles
			Main.Map = new WorldMap(Main.maxTilesX, Main.maxTilesY);

			// Space for more tiles -- Actual tiles
			Main.tile = new Tile[Main.maxTilesX + 1, Main.maxTilesY + 1];
			// Color for each tile

			Main.initMap = new bool[Main.mapTargetX, Main.mapTargetY];
			Main.mapWasContentLost = new bool[Main.mapTargetX, Main.mapTargetY];


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
