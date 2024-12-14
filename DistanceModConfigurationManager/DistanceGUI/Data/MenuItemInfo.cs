using DistanceModConfigurationManager.DistanceGUI.Controls;
using UnityEngine;

namespace DistanceModConfigurationManager.DistanceGUI.Data
{
    public class MenuItemInfo : MonoBehaviour
    {
        public string Id => Item?.Id;
        public MenuItemBase Item { get; set; }
    }
}
