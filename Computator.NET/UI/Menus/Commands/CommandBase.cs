﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Computator.NET.Benchmarking;
using Computator.NET.Config;
using Computator.NET.DataTypes.Localization;
using Computator.NET.Localization;
using Computator.NET.Logging;
using Computator.NET.Properties;
using Computator.NET.UI.CodeEditors;
using Computator.NET.UI.Dialogs;
using Computator.NET.UI.Menus.Commands;
using Computator.NET.UI.MVP;
using Computator.NET.UI.MVP.Views;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Computator.NET.UI.Commands
{


    class LanguageCommand : DropDownCommand<CultureInfo>
    {
        public LanguageCommand()
        {
            Items = new CultureInfo[] {new CultureInfo("en"),
                new CultureInfo("pl"),
                new CultureInfo("de"),
                new CultureInfo("cs")};

            SelectedItem = CultureInfo.CurrentCulture;
            DisplayProperty = "NativeName";
        }

        public override void Execute()
        {
            Thread.CurrentThread.CurrentCulture = SelectedItem;
            LocalizationManager.GlobalUICulture = SelectedItem;
            Settings.Default.Language = SelectedItem;
            Settings.Default.Save();
        }
    }


    abstract class DropDownCommand<T> : CommandBase
    {
        public IEnumerable<T> Items { get; set; }
        public T SelectedItem { get; set; }
        public string DisplayProperty { get; set; }
    }



    class OptionsCommand : CommandBase
    {
        public OptionsCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.optionsToolStripMenuItem1_Text;
            this.ToolTip = MenuStrings.optionsToolStripMenuItem1_Text;
        }


        public override void Execute()
        {
            new SettingsForm().ShowDialog(/*this*/);
        }
    }

    class FullScreenCommand : CommandBase
    {

        private readonly IMainForm mainFormView;

        public FullScreenCommand(IMainForm mainFormView)
        {
            //this.Icon = Resources;
            this.Text = MenuStrings.fullscreenToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.fullscreenToolStripMenuItem_Text;
         //   this.CheckOnClick = true;
            this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            this.Checked = !this.Checked;
            if (this.Checked)
            {
                // this.TopMost = true;
                mainFormView.FormBorderStyle = FormBorderStyle.None;
                mainFormView.WindowState = FormWindowState.Maximized;
            }
            else
            {
                // this.TopMost = false;
                mainFormView.FormBorderStyle = FormBorderStyle.Sizable;
                mainFormView.WindowState = FormWindowState.Normal;
            }
        }
    }

    class LogsCommand : CommandBase
    {
        public LogsCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.Logs_Text;
            this.ToolTip = MenuStrings.Logs_Text;
        }


        public override void Execute()
        {
            if (Directory.Exists(SimpleLogger.LogsDirectory))
                Process.Start(SimpleLogger.LogsDirectory);
            else
                MessageBox.Show(
                    Strings.GUI_logsToolStripMenuItem_Click_You_dont_have_any_logs_yet_);
        }
    }

    class ThanksToCommand : CommandBase
    {
        public ThanksToCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.ThanksTo_Text;
            this.ToolTip = MenuStrings.ThanksTo_Text;
        }


        public override void Execute()
        {
            MessageBox.Show(
                           GlobalConfig.betatesters + Environment.NewLine + Environment.NewLine + GlobalConfig.translators +
                           Environment.NewLine + Environment.NewLine +
                           GlobalConfig.libraries + Environment.NewLine + Environment.NewLine +
                           GlobalConfig.others, Strings.SpecialThanksTo);
        }
    }

    class FeaturesCommand : CommandBase
    {
        public FeaturesCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.Features_Text;
            this.ToolTip = MenuStrings.Features_Text;
        }


        public override void Execute()
        {
            MessageBox.Show(Strings.featuresInclude, Strings.Features);
        }
    }

    class ChangelogCommand : CommandBase
    {
        public ChangelogCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.Changelog_Text;
            this.ToolTip = MenuStrings.Changelog_Text;
        }


        public override void Execute()
        {
            new ChangelogForm().ShowDialog(/*this*/);
        }
    }

    class BugReportingCommand : CommandBase
    {
        public BugReportingCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.BugReporting_Text;
            this.ToolTip = MenuStrings.BugReporting_Text;
        }


        public override void Execute()
        {
            new BugReportingForm().ShowDialog(/*this*/);
        }
    }

    class AboutCommand : CommandBase
    {
        public AboutCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.aboutToolStripMenuItem1_Text;
            this.ToolTip = MenuStrings.aboutToolStripMenuItem1_Text;
        }


        public override void Execute()
        {
            new AboutBox1().ShowDialog(/*this*/);
        }
    }

    class BenchmarkCommand : CommandBase
    {
        public BenchmarkCommand()
        {
            //this.ShortcutKeyString = "Shift+6";
            //this.Icon = Resources.exponentation;
            this.Text = MenuStrings.Benchmark_Text;
            this.ToolTip = MenuStrings.Benchmark_Text;
        }


        public override void Execute()
        {
            new BenchmarkForm().ShowDialog(/*this*/);
        }
    }


    class DummyCommand : CommandBase
    {
        public DummyCommand(string text, string toolTip=null)
        {
            //this.Icon = Resources.runToolStripButtonImage;
            this.Text = text;
            this.ToolTip = toolTip ?? text;
        }
        
        public override void Execute()
        {
            
        }
    }




    class ExponentCommand : CommandBase
    {
        public ExponentCommand()
        {
            this.CheckOnClick = true;
            this.ShortcutKeyString = "Shift+6";
            SharedViewState.Instance.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(SharedViewState.Instance.IsExponent))
                    this.Checked = SharedViewState.Instance.IsExponent;
            };

            this.Icon = Resources.exponentation;
            this.Text = MenuStrings.exponentiationToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.exponentiationToolStripMenuItem_Text;
        }


        public override void Execute()
        {
            SharedViewState.Instance.IsExponent = !SharedViewState.Instance.IsExponent;
        }
    }

    class ExitCommand : CommandBase
    {


        public ExitCommand()
        {
            this.Text = MenuStrings.exitToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.exitToolStripMenuItem_Text;
        }


        public override void Execute()
        {
            Application.Exit();
        }
    }

    class RunCommand : CommandBase
    {


        public RunCommand()
        {
            this.Icon = Resources.runToolStripButtonImage;
            this.Text = MenuStrings.runToolStripButton_Text;
            this.ToolTip = MenuStrings.runToolStripButton_Text;
        }


        public override void Execute()
        {
            SharedViewState.Instance.CurrentAction.Invoke(this, new EventArgs());
        }
    }

    class HelpCommand : CommandBase
    {


        public HelpCommand()
        {
            this.Icon = Resources.helpToolStripButtonImage;
            this.Text = MenuStrings.helpToolStripButton_Text;
            this.ToolTip = MenuStrings.helpToolStripButton_Text;
        }


        public override void Execute()
        {
            new AboutBox1().ShowDialog();
        }
    }

    class PrintPreviewCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;

        private IMainForm mainFormView;

        public PrintPreviewCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            this.Icon = Resources.printPreviewToolStripMenuItemImage;
            this.Text = MenuStrings.printPreviewToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.printPreviewToolStripMenuItem_Text;

            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            switch ((int)SharedViewState.Instance.CurrentView)
            {
                case 0:
                    //if (_calculationsMode == CalculationsMode.Real)
                    mainFormView.ChartingView.Charts[SharedViewState.Instance.CalculationsMode].PrintPreview();
                    // else
                    //  SendStringAsKey("^P");
                    break;

                case 4:
                    //scriptingCodeEditor();

                    break;

                case 5:
                    //this.customFunctionsCodeEditor
                    break;

                default:
                    mainFormView.SendStringAsKey("^P"); //this.chart2d.Printing.PrintPreview();
                    break;
            }

        }
    }
    class PrintCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;

        private IMainForm mainFormView;

        public PrintCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            this.Icon = Resources.printToolStripButtonImage;
            this.Text = MenuStrings.printToolStripButton_Text;
            this.ToolTip = MenuStrings.printToolStripButton_Text;
            this.ShortcutKeyString = "Ctrl+P";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            switch ((int)SharedViewState.Instance.CurrentView)
            {
                case 0:
                    //if (_calculationsMode == CalculationsMode.Real)
                    mainFormView.ChartingView.Charts[SharedViewState.Instance.CalculationsMode].Print();
                    // else
                    //  SendStringAsKey("^P");
                    break;

                case 4:
                    //scriptingCodeEditor();

                    break;

                case 5:
                    //this.customFunctionsCodeEditor
                    break;

                default:
                    mainFormView.SendStringAsKey("^P"); //this.chart2d.Printing.PrintPreview();
                    break;
            }

        }
    }

    class UndoCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;
        private IMainForm mainFormView;

        public UndoCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            //this.Icon = Resources.copyToolStripButtonImage;
            this.Text = MenuStrings.undoToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.undoToolStripMenuItem_Text;
            this.ShortcutKeyString = "Ctrl+Z";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            if ((int)SharedViewState.Instance.CurrentView < 4)
                mainFormView.SendStringAsKey("^Z"); //expressionTextBox.Undo();
            else if ((int)SharedViewState.Instance.CurrentView == 4)
            {
                if (scriptingCodeEditor.Focused)
                    scriptingCodeEditor.Undo();
                else
                    mainFormView.SendStringAsKey("^Z");
            }
            else
            {
                if (customFunctionsCodeEditor.Focused)
                    customFunctionsCodeEditor.Undo();
                else
                    mainFormView.SendStringAsKey("^Z");
            }

            mainFormView.SendStringAsKey("^Z");
        }
    }

    class RedoCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;
        private IMainForm mainFormView;

        public RedoCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            //this.Icon = Resources.copyToolStripButtonImage;
            this.Text = MenuStrings.redoToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.redoToolStripMenuItem_Text;
            this.ShortcutKeyString = "Ctrl+Y";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            if ((int)SharedViewState.Instance.CurrentView < 4)
            {
                mainFormView.SendStringAsKey("^Y");
                //expressionTextBox.do()
            }
            else if ((int)SharedViewState.Instance.CurrentView == 4)
            //scriptingCodeEditor.Focus();
            {
                if (scriptingCodeEditor.Focused)
                    scriptingCodeEditor.Redo();
                else
                    mainFormView.SendStringAsKey("^Y");
            }
            else
            {
                if (customFunctionsCodeEditor.Focused)
                    customFunctionsCodeEditor.Redo();
                else
                    mainFormView.SendStringAsKey("^Y");
            }

            mainFormView.SendStringAsKey("^Y");
        }
    }



    class SelectAllCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;
        private IMainForm mainFormView;

        public SelectAllCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            //this.Icon = Resources.copyToolStripButtonImage;
            this.Text = MenuStrings.selectAllToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.selectAllToolStripMenuItem_Text;
            this.ShortcutKeyString = "Ctrl+A";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            if ((int)SharedViewState.Instance.CurrentView < 4)
            {
                mainFormView.SendStringAsKey("^A"); //expressionTextBox.SelectAll();
            }
            else if ((int)SharedViewState.Instance.CurrentView == 4)
            {
                if (scriptingCodeEditor.Focused)
                    scriptingCodeEditor.SelectAll();
                else
                    mainFormView.SendStringAsKey("^A");
            }
            else
            {
                if (customFunctionsCodeEditor.Focused)
                    customFunctionsCodeEditor.SelectAll();
                //  else
                mainFormView.SendStringAsKey("^A");
            }

            mainFormView.SendStringAsKey("^A");
        }
    }






    class CutCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;
        private IMainForm mainFormView;

        public CutCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            this.Icon = Resources.cutToolStripButtonImage;
            this.Text = MenuStrings.cutToolStripButton_Text;
            this.ToolTip = MenuStrings.cutToolStripButton_Text;
            this.ShortcutKeyString = "Ctrl+X";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {

            switch ((int)SharedViewState.Instance.CurrentView)
            {
                case 4:
                    if (scriptingCodeEditor.Focused)
                        scriptingCodeEditor.Cut();
                    else
                        mainFormView.SendStringAsKey("^X");
                    break;

                case 5:
                    if (customFunctionsCodeEditor.Focused)
                        customFunctionsCodeEditor.Cut();
                    else
                        mainFormView.SendStringAsKey("^X");
                    break;

                default: //if ((int)SharedViewState.Instance.CurrentView < 4)
                    mainFormView.SendStringAsKey("^X"); //expressionTextBox.Cut();
                    break;
            }
        }
    }

    class CopyCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;
        private IMainForm mainFormView;

        public CopyCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            this.Icon = Resources.copyToolStripButtonImage;
            this.Text = MenuStrings.copyToolStripButton_Text;
            this.ToolTip = MenuStrings.copyToolStripButton_Text;
            this.ShortcutKeyString = "Ctrl+C";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {

            if ((int)SharedViewState.Instance.CurrentView < 4)
            {
                mainFormView.SendStringAsKey("^C"); //expressionTextBox.Copy();
            }
            else if ((int)SharedViewState.Instance.CurrentView == 4)
            {
                if (scriptingCodeEditor.Focused)
                    scriptingCodeEditor.Copy();
                else
                    mainFormView.SendStringAsKey("^C");
            }
            else
            {
                if (customFunctionsCodeEditor.Focused)
                    customFunctionsCodeEditor.Copy();
                else
                    mainFormView.SendStringAsKey("^C");
            }
        }
    }

    class PasteCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;
        private IMainForm mainFormView;

        public PasteCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor, IMainForm mainFormView)
        {
            this.ShortcutKeyString = "Ctrl+V";
            this.Icon = Resources.pasteToolStripButtonImage;
            this.Text = MenuStrings.pasteToolStripButton_Text;
            this.ToolTip = MenuStrings.pasteToolStripButton_Text;

            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.mainFormView = mainFormView;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {

            if ((int)SharedViewState.Instance.CurrentView < 4)
            {
                mainFormView.SendStringAsKey("^V"); //expressionTextBox.Paste();
            }
            else if ((int)SharedViewState.Instance.CurrentView == 4)
            {
                if (scriptingCodeEditor.Focused)
                    scriptingCodeEditor.Paste();
                else
                    mainFormView.SendStringAsKey("^V");
            }
            else
            {
                if (customFunctionsCodeEditor.Focused)
                    customFunctionsCodeEditor.Paste();
                else
                    mainFormView.SendStringAsKey("^V");
            }
        }
    }

    class SaveAsCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;


        public SaveAsCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor)
        {
          //  this.Icon = Resources.save;
            this.Text = MenuStrings.saveAsToolStripMenuItem_Text;
            this.ToolTip = MenuStrings.saveAsToolStripMenuItem_Text;

            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {

            switch ((int)SharedViewState.Instance.CurrentView)
            {
                case 0:

                    //mainFormView.SendStringAsKey("^S");
                    break;

                case 4:
                    scriptingCodeEditor.SaveAs();
                    break;

                case 5:
                    customFunctionsCodeEditor.SaveAs();
                    break;

                default:
                    //mainFormView.SendStringAsKey("^S");
                    break;
            }
        }
    }

    class SaveCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;


        public SaveCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor)
        {
            this.ShortcutKeyString = "Ctrl+S";
            this.Icon = Resources.saveToolStripButtonImage;
            this.Text = MenuStrings.saveToolStripButton_Text;
            this.ToolTip = MenuStrings.saveToolStripButton_Text;

            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {

            switch ((int)SharedViewState.Instance.CurrentView)
            {
                case 0:

                    //mainFormView.SendStringAsKey("^S");
                    break;

                case 4:
                     scriptingCodeEditor.Save();
                    break;

                case 5:
                    customFunctionsCodeEditor.Save();
                    break;

                default:
                    //mainFormView.SendStringAsKey("^S");
                    break;
            }

           // mainFormView.SendStringAsKey("^S");
        }
    }

    class OpenCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;


        public OpenCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor)
        {
            this.Icon = Resources.openToolStripButtonImage;
            this.Text = MenuStrings.openToolStripButton_Text;
            this.ToolTip = MenuStrings.openToolStripButton_Text;
            this.ShortcutKeyString = "Ctrl+O";
            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            var ofd = new OpenFileDialog { Filter = GlobalConfig.tslFilesFIlter };//TODO: move this to mainView or something
            if (ofd.ShowDialog() != DialogResult.OK)
                return;



            switch ((int)SharedViewState.Instance.CurrentView)
            {
                case 0:

                    //mainFormView.SendStringAsKey("^S");
                    break;

                case 4:
                    scriptingCodeEditor.NewDocument(ofd.FileName);
                    break;

                case 5:
                    customFunctionsCodeEditor.NewDocument(ofd.FileName);
                    break;

                default:
                    //mainFormView.SendStringAsKey("^S");
                    break;
            }

            //mainFormView.SendStringAsKey("^S");
        }
    }

    class NewCommand : CommandBase
    {
        private ICanFileEdit scriptingCodeEditor;
        private ICanFileEdit customFunctionsCodeEditor;


        public NewCommand(ICanFileEdit scriptingCodeEditor, ICanFileEdit customFunctionsCodeEditor)
        {
            this.Icon = Resources.newToolStripButtonImage;
            this.Text = MenuStrings.newToolStripButton_Text;
            this.ToolTip = MenuStrings.newToolStripButton_Text;

            this.scriptingCodeEditor = scriptingCodeEditor;
            this.customFunctionsCodeEditor = customFunctionsCodeEditor;
            this.ShortcutKeyString = "Ctrl+N";
            // this.mainFormView = mainFormView;
        }


        public override void Execute()
        {
            switch ((int)SharedViewState.Instance.CurrentView)
            {
                case 0:

                    //mainFormView.SendStringAsKey("^S");
                    break;

                case 4:
                    scriptingCodeEditor.NewDocument();
                    break;

                case 5:
                    customFunctionsCodeEditor.NewDocument();
                    break;

                default:
                    //mainFormView.SendStringAsKey("^S");
                    break;
            }

            //mainFormView.SendStringAsKey("^S");
        }
    }

    internal abstract class CommandBase : IToolbarCommand
    {
        private bool _checked;
        private bool _checkOnClick;

        private ToolStripItemImageScaling _imageScaling;
        private ToolStripItemDisplayStyle _displayStyle;

        private string _text;
        private Image icon;
        private bool isEnabled;
        private string toolTip;

        protected CommandBase()
        {
            isEnabled = true;
            _checked = _checkOnClick = false;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        private string _ShortcutKeyString;
        public string ShortcutKeyString
        {
            get { return _ShortcutKeyString; }
            set
            {
                if (_ShortcutKeyString != value)
                {
                    _ShortcutKeyString = value;
                    OnPropertyChanged(nameof(ShortcutKeyString));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Execute();

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        public Image Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged("Icon");
                }
            }
        }

        public string ToolTip
        {
            get { return toolTip; }
            set
            {
                if (toolTip != value)
                {
                    toolTip = value;
                    OnPropertyChanged("ToolTip");
                }
            }
        }

        public Keys ShortcutKey { get; set; }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    OnPropertyChanged(nameof(Checked));
                }
            }
        }

        public bool CheckOnClick
        {
            get { return _checkOnClick; }
            set
            {
                if (_checkOnClick != value)
                {
                    _checkOnClick = value;
                    OnPropertyChanged(nameof(CheckOnClick));
                }
            }
        }

        public IEnumerable<IToolbarCommand> ChildrenCommands { get; set; }

        public ToolStripItemImageScaling ImageScaling
        {
            get { return _imageScaling; }
            set
            {
                if (_imageScaling != value)
                {
                    _imageScaling = value;
                    OnPropertyChanged(nameof(ImageScaling));
                }
            }
        }

        public ToolStripItemDisplayStyle DisplayStyle
        {
            get { return _displayStyle; }
            set
            {
                if (_displayStyle != value)
                {
                    _displayStyle = value;
                    OnPropertyChanged(nameof(DisplayStyle));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}