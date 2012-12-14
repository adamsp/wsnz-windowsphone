using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Localization;

namespace TileControls
{
    public class FlipTileController
    {
        private string DefaultTileTitle;

        public FlipTileController() 
        {
            DefaultTileTitle = AppResources.LiveTileDefaultTitle;
        }

        /// <summary>
        /// Updates the FlipTile with appropriate data from a collection of quakes.
        /// </summary>
        /// <param name="quakes"></param>
        public void UpdateFlipTile(IEnumerable<Earthquake> quakes)
        {
            // Following code found here: http://stackoverflow.com/questions/8027812/can-i-update-a-live-tile-in-mango-using-local-data
            FlipTileData newTileData;
            TimeSpan oneDay = new TimeSpan(24, 0, 0);
            DateTime yesterday = DateTime.Now.Subtract(oneDay);

            List<Earthquake> latestQuakes = quakes.Where(q => (DateTime.Compare(q.Date, yesterday) > 0)).ToList();

            if (latestQuakes.Count > 0)
            {
                Earthquake latestQuake = latestQuakes.First();
                newTileData = new FlipTileData
                {
                    Title = String.Format(AppResources.LiveTileTitleFormat, latestQuakes.Count),
                    BackTitle = String.Format(AppResources.LiveTileTitleFormat, latestQuakes.Count),
                    BackContent = String.Format(AppResources.LiveTileBackContentFormat, latestQuake.FormattedMagnitude, latestQuake.FormattedDepth),
                    WideBackContent = String.Format(AppResources.LiveTileWideBackContentFormat, latestQuake.FormattedMagnitude, latestQuake.FormattedDepth, latestQuake.RelativeLocation),
                };
            }
            else
            {
                newTileData = new FlipTileData
                {
                    Title = DefaultTileTitle,
                    BackTitle = string.Empty,
                    BackContent = AppResources.LiveTileBackContentNoQuakes,
                    WideBackContent = AppResources.LiveTileBackContentNoQuakes,
                };
            }
            UpdateTile(newTileData);
        }

        private void UpdateTile(FlipTileData tileData)
        {
            // Application Tile is always the first Tile, even if it is not pinned to Start
            ShellTile tileToFind = ShellTile.ActiveTiles.First();
            // Application Tile should always be found
            if (tileToFind != null)
            {
                // update the Application Tile
                tileToFind.Update(tileData);
            }
        }

        /// <summary>
        /// Clears the FlipTile of all back content, effectively rendering it a
        /// static/non-live tile.
        /// </summary>
        public void ClearFlipTile()
        {
            var newTileData = new FlipTileData
            {
                Title = DefaultTileTitle,
                BackTitle = string.Empty,
                BackContent = string.Empty,
                WideBackContent = string.Empty,
            };
            UpdateTile(newTileData);
        }

    }
}
