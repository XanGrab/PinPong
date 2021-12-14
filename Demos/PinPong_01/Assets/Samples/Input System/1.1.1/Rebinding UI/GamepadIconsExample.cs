using System;
using UnityEngine.UI;

////TODO: have updateBindingUIEvent receive a control path string, too (in addition to the device layout name)

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// This is an example for how to override the default display behavior of bindings. The component
    /// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
    /// of a binding should be refreshed. It then checks whether we have an icon for the current binding
    /// and if so, replaces the default text display with an icon.
    /// </summary>
    public class GamepadIconsExample : MonoBehaviour
    {
        public GamepadIcons keyboard;
        public GamepadIcons xbox;
        public GamepadIcons ps4;

        protected void OnEnable()
        {
            // Hook into all updateBindingUIEvents on all RebindActionUI components in our hierarchy.
            var rebindUIComponents = transform.GetComponentsInChildren<RebindActionUI>();
            foreach (var component in rebindUIComponents)
            {
                component.updateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
                component.UpdateBindingDisplay();
            }
        }

        protected void OnUpdateBindingDisplay(RebindActionUI component, string bindingDisplayString, string deviceLayoutName, string controlPath)
        {
            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return;

            var icon = default(Sprite);
            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
                icon = ps4.GetSprite(controlPath);
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
                icon = xbox.GetSprite(controlPath);
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Keyboard"))
                icon = keyboard.GetSprite(controlPath);

            var textComponent = component.bindingText;

            // Grab Image component.
            var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
            var imageComponent = imageGO.GetComponent<Image>();

            if (icon != null)
            {
                textComponent.gameObject.SetActive(false);
                imageComponent.sprite = icon;
                imageComponent.gameObject.SetActive(true);
            }
            else
            {
                textComponent.gameObject.SetActive(true);
                imageComponent.gameObject.SetActive(false);
            }
        }

        [Serializable]
        public struct GamepadIcons
        {
            public Sprite buttonSouth;
            public Sprite buttonNorth;
            public Sprite buttonEast;
            public Sprite buttonWest;
            public Sprite startButton;
            public Sprite selectButton;
            public Sprite leftTrigger;
            public Sprite rightTrigger;
            public Sprite leftShoulder;
            public Sprite rightShoulder;
            public Sprite dpad;
            public Sprite dpadUp;
            public Sprite dpadDown;
            public Sprite dpadLeft;
            public Sprite dpadRight;
            public Sprite leftStick;
            public Sprite rightStick;
            public Sprite leftStickPress;
            public Sprite rightStickPress;
            public Sprite aKey;
            public Sprite bKey;
            public Sprite cKey;
            public Sprite dKey;
            public Sprite eKey;
            public Sprite fKey;
            public Sprite gKey;
            public Sprite hKey;
            public Sprite iKey;
            public Sprite jKey;
            public Sprite kKey;
            public Sprite lKey;
            public Sprite mKey;
            public Sprite nKey;
            public Sprite oKey;
            public Sprite pKey;
            public Sprite qKey;
            public Sprite rKey;
            public Sprite sKey;
            public Sprite tKey;
            public Sprite uKey;
            public Sprite vKey;
            public Sprite wKey;
            public Sprite xKey;
            public Sprite yKey;
            public Sprite zKey;

            public Sprite GetSprite(string controlPath)
            {
                // From the input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.
                switch (controlPath)
                {
                    case "buttonSouth": return buttonSouth;
                    case "buttonNorth": return buttonNorth;
                    case "buttonEast": return buttonEast;
                    case "buttonWest": return buttonWest;
                    case "start": return startButton;
                    case "select": return selectButton;
                    case "leftTrigger": return leftTrigger;
                    case "rightTrigger": return rightTrigger;
                    case "leftShoulder": return leftShoulder;
                    case "rightShoulder": return rightShoulder;
                    case "dpad": return dpad;
                    case "dpad/up": return dpadUp;
                    case "dpad/down": return dpadDown;
                    case "dpad/left": return dpadLeft;
                    case "dpad/right": return dpadRight;
                    case "leftStick": return leftStick;
                    case "rightStick": return rightStick;
                    case "leftStickPress": return leftStickPress;
                    case "rightStickPress": return rightStickPress;
                    case "aKey": return aKey;
                    case "bKey": return bKey;
                    case "cKey": return cKey;
                    case "dKey": return dKey;
                    case "eKey": return eKey;
                    case "fKey": return fKey;
                    case "gKey": return gKey;
                    case "hKey": return hKey;
                    case "iKey": return iKey;
                    case "jKey": return jKey;
                    case "kKey": return kKey;
                    case "lKey": return lKey;
                    case "mKey": return mKey;
                    case "nKey": return nKey;
                    case "oKey": return oKey;
                    case "pKey": return pKey;
                    case "qKey": return qKey;
                    case "rKey": return rKey;
                    case "sKey": return sKey;
                    case "tKey": return tKey;
                    case "uKey": return uKey;
                    case "vKey": return vKey;
                    case "wKey": return wKey;
                    case "xKey": return xKey;
                    case "yKey": return yKey;
                    case "zKey": return zKey;
                }
                return null;
            }
        }
    }
}
