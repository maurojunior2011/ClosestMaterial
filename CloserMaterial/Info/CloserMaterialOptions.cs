using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace CloserMaterial.Info
{
    [JsonObject]
    public class CloserMaterialOptions
    {
        [Option("Available Material Closest", "The will appear in-game as available to be used in blueprint as a construction planner.", null)]
        public bool MaterialAvailable { get; set; } = true;

        [Option("Any Blueprint", "The tool will change the materials of all blueprints or just the ones that have this MOD's material.", null)]
        public bool AllBlueprints { get; set; } = false;
    }
}
