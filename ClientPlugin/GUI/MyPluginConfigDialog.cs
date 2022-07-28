using System;
using Sandbox;
using Sandbox.Graphics.GUI;
using Shared.Plugin;
using VRage;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.GUI
{

    public class MyPluginConfigDialog : MyGuiScreenBase
    {
        private const string Caption = "Configure Cosmic Wine Fixes";
        public override string GetFriendlyName() => "MyPluginConfigDialog";

        private MyLayoutTable layoutTable;

        private MyGuiControlLabel enabledLabel;
        private MyGuiControlCheckbox enabledCheckbox;

        private MyGuiControlLabel clipboardFixEnabledLabel;
        private MyGuiControlCheckbox clipboardFixEnabledCheckbox;

        private MyGuiControlLabel threadDelayLabel;
        private MyGuiControlSlider threadDelaySlider;

        private MyGuiControlLabel clipboardRetriesLabel;
        private MyGuiControlSlider clipboardRetriesSlider;

        private MyGuiControlLabel openLogEnabledLabel;
        private MyGuiControlCheckbox openLogEnabledCheckbox;

        private MyGuiControlLabel showExitToLinuxLabel;
        private MyGuiControlCheckbox showExitToLinuxCheckbox;

        private MyGuiControlButton closeButton;

        public MyPluginConfigDialog() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.8f, 0.6f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            CreateControls();
            LayoutControls();
        }

        private void CreateControls()
        {
            AddCaption(Caption);

            var config = Common.Config;

            CreateCheckbox(out enabledLabel,
                out enabledCheckbox,
                config.Enabled,
                value => config.Enabled = value,
                "Enabled",
                "Enables the plugin");

            CreateCheckbox(out clipboardFixEnabledLabel,
                out clipboardFixEnabledCheckbox,
                config.ClipboardFixEnabled,
                value => config.ClipboardFixEnabled = value,
                "Fix clipboard",
                "Enables the clipboard fix\nRestart the game after enabling this");

            CreateSlider(out threadDelayLabel,
                out threadDelaySlider,
                10,
                100,
                config.ThreadExecutionIntervalMs,
                value => config.ThreadExecutionIntervalMs = value,
                "Loop execution delay",
                " ms");

            CreateSlider(out clipboardRetriesLabel,
                out clipboardRetriesSlider,
                1,
                10,
                config.MaxCopyRetries,
                value => config.MaxCopyRetries = value,
                "Copy to clipboard max retries");

            CreateCheckbox(out openLogEnabledLabel,
                out openLogEnabledCheckbox,
                config.LogOpeningEnabled,
                value => config.LogOpeningEnabled = value,
                "Open log on crash",
                "Automatically open log in notepad when the game crashes\nUseful when the log link in the crash window doesn't work");

            CreateCheckbox(out showExitToLinuxLabel,
                out showExitToLinuxCheckbox,
                config.ShowExitToLinux,
                value => config.ShowExitToLinux = value,
                "Exit to Linux",
                "Show Exit to Linux instead of Windows\nThis option is English-only and requires a game restart");

            closeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OnOk);
        }

        private void OnOk(MyGuiControlButton _) => CloseScreen();

        private void CreateCheckbox(out MyGuiControlLabel labelControl, out MyGuiControlCheckbox checkboxControl, bool value, Action<bool> store, string label, string tooltip)
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
            };

            checkboxControl = new MyGuiControlCheckbox(toolTip: tooltip)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = true,
                IsChecked = value
            };
            checkboxControl.IsCheckedChanged += cb => store(cb.IsChecked);
        }

        private void CreateSlider(out MyGuiControlLabel labelControl, out MyGuiControlSlider sliderControl, int minValue, int maxValue, int value, Action<int> store, string label, string tooltipPostfix = "")
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
            };

            sliderControl = new MyGuiControlSlider(toolTip: value + tooltipPostfix, width: 0.25f)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = true,
                IntValue = true,
                MinValue = minValue,
                MaxValue = maxValue,
                Value = value
            };
            sliderControl.ValueChanged += sl =>
            {
                store((int)sl.Value);
                sl.SetToolTip(sl.Value + tooltipPostfix);
            };
        }

        private void LayoutControls()
        {
            var size = Size ?? Vector2.One;
            layoutTable = new MyLayoutTable(this, -0.3f * size, 0.6f * size);
            layoutTable.SetColumnWidths(400f, 400f);
            layoutTable.SetRowHeights(90f, 50f, 50f, 50f, 90f, 50f, 150f);

            var row = 0;

            layoutTable.Add(enabledLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(enabledCheckbox, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;

            layoutTable.Add(clipboardFixEnabledLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(clipboardFixEnabledCheckbox, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;

            layoutTable.Add(threadDelayLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(threadDelaySlider, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;

            layoutTable.Add(clipboardRetriesLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(clipboardRetriesSlider, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;

            layoutTable.Add(openLogEnabledLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(openLogEnabledCheckbox, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;

            layoutTable.Add(showExitToLinuxLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(showExitToLinuxCheckbox, MyAlignH.Left, MyAlignV.Center, row, 1);
            row++;


            layoutTable.Add(closeButton, MyAlignH.Center, MyAlignV.Center, row, 0, colSpan: 2);
            // row++;
        }
    }
}
