using System;
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
                Thread.Sleep(1000);
                var now = DateTime.Now;

                foreach (var command in Context.ProgramData.Commands)
                {
                    bool executed = false;

                    foreach (var trigger in command.Triggers)
                    {
                        if (trigger.triggertype == Types.TriggersType.Time)
                        {
                            DateTime triggerTime = DateTime.Parse(trigger.textvalue);

                            if (trigger.textvalue.Contains('.') && trigger.textvalue.Contains(':'))
                            {
                                if (now == triggerTime)
                                {
                                    command.ExecuteCommand();
                                    executed = true;
                                    break;
                                }
                            }
                            else if (!trigger.textvalue.Contains('.') && trigger.textvalue.Contains(':'))
                            {
                                if (now.Hour == triggerTime.Hour && now.Minute == triggerTime.Minute && now.Second == triggerTime.Second)
                                {
                                    command.ExecuteCommand();
                                    executed = true;
                                    break;
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
                            DateTime triggerTime = DateTime.Parse(trigger.textvalue);
                            if (trigger.textvalue.Contains('.') && trigger.textvalue.Contains(':'))
                            {
                                if (now.Date == triggerTime.Date && now.Hour == triggerTime.Hour && now.Minute == triggerTime.Minute && now.Second == triggerTime.Second)
                                {
                                    script.ExecuteScript();
                                    executed = true;
                                    break;
                                }
                            }
                            else if (!trigger.textvalue.Contains('.') && trigger.textvalue.Contains(':'))
                            {
                                if (now.Hour == triggerTime.Hour && now.Minute == triggerTime.Minute && now.Second == triggerTime.Second)
                                {
                                    script.ExecuteScript();
                                    executed = true;
                                    break;
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
