using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class BackgroundService
    {
        public static void StartServise()
        {
            Task.Run(() => { Servise(); });
        }

        private static void Servise()
        {
            while (true)
            {
                Thread.Sleep(555);
                var now = DateTime.Now;

                foreach (var command in Context.ProgramData.Commands)
                {
                    bool executed = false;

                    foreach (var trigger in command.Triggers)
                    {
                        if (trigger.triggertype == Types.TriggersType.Time)
                        {
                            if (trigger.textvalue.Length == 7)
                            {
                                var time = trigger.textvalue.Substring(2);
                                if (DateTime.TryParse(time, out DateTime timetrigger))
                                {
                                    if (now.Hour == timetrigger.Hour && now.Minute == timetrigger.Minute && now.Second == timetrigger.Second)
                                    {
                                        command.ExecuteCommandAsync();
                                        executed = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                var moment = trigger.textvalue.Split(" в");
                                if (Program.DaysOfWeek.Contains(moment[0]))
                                {
                                    var day = Array.IndexOf(Program.DaysOfWeek, moment[0]);
                                    if ((int)now.DayOfWeek == day)
                                    {
                                        var time = moment[1];
                                        if (DateTime.TryParse(time, out DateTime timetrigger))
                                        {
                                            if (now.Hour == timetrigger.Hour && now.Minute == timetrigger.Minute && now.Second == timetrigger.Second)
                                            {
                                                command.ExecuteCommandAsync();
                                                executed = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (DateTime.TryParse(trigger.textvalue.Replace("в ", ""), out DateTime date))
                                    {
                                        if (now.Date == date.Date)
                                        {
                                            if (now.Hour == date.Hour && now.Minute == date.Minute && now.Second == date.Second)
                                            {
                                                command.ExecuteCommandAsync();
                                                executed = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (trigger.triggertype == Types.TriggersType.StartApp)
                        {
                            // Обработка триггера типа StartApp (если необходимо)
                        }
                    }

                    if (executed)
                    {
                        continue;
                    }
                }

                foreach (var script in Context.ProgramData.Scripts)
                {
                    bool executed = false;

                    foreach (var trigger in script.Triggers)
                    {
                        if (trigger.triggertype == Types.TriggersType.Time)
                        {
                            if (trigger.textvalue.Length == 7)
                            {
                                var time = trigger.textvalue.Substring(2);
                                if (DateTime.TryParse(time, out DateTime timetrigger))
                                {
                                    if (now.Hour == timetrigger.Hour && now.Minute == timetrigger.Minute && now.Second == timetrigger.Second)
                                    {
                                        script.ExecuteScriptAsync();
                                        executed = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                var moment = trigger.textvalue.Split(" в");
                                if (Program.DaysOfWeek.Contains(moment[0]))
                                {
                                    var day = Array.IndexOf(Program.DaysOfWeek, moment[0]);
                                    if ((int)now.DayOfWeek == day)
                                    {
                                        var time = moment[1];
                                        if (DateTime.TryParse(time, out DateTime timetrigger))
                                        {
                                            if (now.Hour == timetrigger.Hour && now.Minute == timetrigger.Minute && now.Second == timetrigger.Second)
                                            {
                                                script.ExecuteScriptAsync();
                                                executed = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (DateTime.TryParse(trigger.textvalue.Replace("в ", ""), out DateTime date))
                                    {
                                        if (now.Date == date.Date)
                                        {
                                            if (now.Hour == date.Hour && now.Minute == date.Minute && now.Second == date.Second)
                                            {
                                                script.ExecuteScriptAsync();
                                                executed = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (trigger.triggertype == Types.TriggersType.StartApp)
                        {
                            // Обработка триггера типа StartApp (если необходимо)
                        }
                    }

                    if (executed)
                    {
                        continue;
                    }
                }
            }
        }
    }
}
