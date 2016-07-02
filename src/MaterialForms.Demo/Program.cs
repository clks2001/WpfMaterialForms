﻿using System;
using System.Threading.Tasks;

namespace MaterialForms.Demo
{
    public class Program
    {
        [STAThread]
        private static void Main()
        {
            var nameSchema = new StringSchema();
            var window = new MaterialWindow(new MaterialDialog
            {
                Title = "Demo material forms",
                Form = new MaterialForm
                {
                    new CommandSchema
                    {
                        Name = "Login window",
                        CommandHint = "SHOW",
                        Callback = ShowLogin
                    },
                    new CommandSchema
                    {
                        Name = "Settings window",
                        CommandHint = "SHOW",
                        Callback = ShowDemo(SettingsDialog)
                    },
                    new CommandSchema
                    {
                        Name = "2x settings window (data binding)",
                        CommandHint = "SHOW",
                        Callback = ShowDemo(SettingsDialog, true)
                    },
                    new CommandSchema
                    {
                        Name = "Dialog inside window",
                        CommandHint = "SHOW",
                        Callback = async args =>
                        {
                            await ((WindowSession)args.Session).ShowDialog(new MaterialDialog
                            {
                                Message = "Discard draft?",
                                PositiveAction = "DISCARD"
                            }, 250d);
                        }
                    },
                    new CommandSchema
                    {
                        Name = "E-mail window",
                        CommandHint = "SHOW",
                        Callback = ShowDemo(EmailDialog)
                    },
                    new CommandSchema
                    {
                        Name = "Modal 2x schema (data binding)",
                        CommandHint = "SHOW",
                        Callback = async args => await ((WindowSession)args.Session).ShowDialog(new MaterialDialog(nameSchema, nameSchema), 250d)
                    }
                }
            });

            window.Show();
            MaterialApplication.RunDispatcher();
        }

        public static async void ShowLogin(object nothing)
        {
            var window = new MaterialWindow(new MaterialDialog
            {
                Title = "Please log in to continue",
                Form = new MaterialForm
                {
                    new StringSchema
                    {
                        Name = "Username",
                        Key = "user",
                        IconKind = IconKind.Account,
                        Validation = Validators.IsNotEmpty
                    },
                    new PasswordSchema
                    {
                        Name = "Password",
                        Key = "pass",
                        IconKind = IconKind.Key
                    },
                    new BooleanSchema
                    {
                        Name = "Remember me",
                        IsCheckBox = true
                    }
                },
                PositiveAction = "LOG IN",
                OnPositiveAction = async session =>
                {
                    // Simulate asynchronous work
                    await Task.Delay(2000);
                    if ((string)session["user"] == "test" && (string)session["pass"] == "123456")
                    {
                        session.Close(true);
                    }
                    else
                    {
                        await ((WindowSession)session).Alert("Invalid username or password.");
                    }
                },
                ShowsProgressOnPositiveAction = true
            });

            var result = await window.Show();
            if (result == true)
            {
                var formData = window.Dialog.Form.GetValuesList();
                await WindowFactory.Alert($"Username: {formData[0]}\nPassword: {formData[1]}\nRemember me: {formData[2]}", "Positive").Show();
            }
            else
            {
                await WindowFactory.Alert("Dialog dismissed", "Negative").Show();
            }
        }

        public static Action<object> ShowDemo(Func<MaterialDialog> dialog, bool showTwice = false)
        {
            Action<object> callback = arg =>
            {
                var window = new MaterialWindow
                {
                    Dialog = dialog(),
                };

                window.Show();
                if (showTwice)
                {
                    window.Show();
                }
            };

            return callback;
        }

        public static MaterialDialog SettingsDialog()
        {
            return new MaterialDialog
            {
                Title = "Settings",
                Form = new MaterialForm
                {
                    new StringSchema
                    {
                        Name = "Device name",
                        Value = "Android"
                    },
                    new CaptionSchema
                    {
                        Name = "Connectivity"
                    },
                    new BooleanSchema
                    {
                        Name = "WiFi",
                        IconKind = IconKind.Wifi,
                        Value = true
                    },
                    new BooleanSchema
                    {
                        Name = "Mobile Data",
                        IconKind = IconKind.Signal
                    },
                    new CaptionSchema
                    {
                        Name = "Device"
                    },
                    new NumberRangeSchema
                    {
                        Name = "Volume",
                        IconKind = IconKind.VolumeHigh,
                        MinValue = 0,
                        MaxValue = 10,
                        Value = 5
                    },
                    new KeyValueSchema
                    {
                        Name = "Ringtone",
                        Value = "Over the horizon",
                        IconKind = IconKind.MusicNote
                    }
                }
            };
        }

        public static MaterialDialog EmailDialog()
        {
            return new MaterialDialog
            {
                Title = "Send e-mail",
                PositiveAction = "SEND",
                Form = new MaterialForm
                {
                    new StringSchema
                    {
                        Name = "To",
                        IconKind = IconKind.Email
                    },
                    new StringSchema
                    {
                        Name = "Message",
                        IsMultiLine = true,
                        IconKind = IconKind.Comment
                    }
                }
            };
        }
    }
}
