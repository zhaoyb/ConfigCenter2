using System;
using System.Threading;

namespace ConfigCenter
{
    public class Task
    {

        private readonly Thread _worker;
        private bool _isStop;

        private readonly Action<string> _exec;
        private readonly int _interval;
        private readonly int _delay;
        private readonly string _param;
        public Task(Action<string> exec, string param, int interval, int delay)
        {
            try
            {
                _param = param;
                _exec = exec;
                _interval = interval;
                _delay = delay;
                _worker = new Thread(WorkerMethod);
                _worker.Start();
            }
            catch (Exception ex)
            {

            }

        }

        public void Stop()
        {
            _isStop = true;
            if (_worker != null)
            {
                _worker.Interrupt();
            }
        }

        private void WorkerMethod()
        {

            if (_delay > 0)
                Thread.CurrentThread.Join(_delay);

            while (!_isStop)
            {
                if (!_isStop)
                {
                    try
                    {
                        _exec(_param);
                    }
                    catch (Exception ex)
                    {
                    }
                }

                try
                {
                    if (!_isStop)
                    {
                        Thread.CurrentThread.Join(_interval);
                    }
                }
                catch (ThreadInterruptedException)
                {
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
