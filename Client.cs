using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using EdgeJs;

namespace Epic.Socket.IO
{
    public class Client
    {

        public static Client io(string url, string sciprt = @"return require('../js/index.js')", dynamic opts = null)
        {
            if (opts == null)
                opts = new ExpandoObject();

            opts.url = url;
            

            var result = new Client();
            result.opts = opts;
            result.script = sciprt;
            result.start();

            return result;
        }

        Client(string url = null)
        {
            this.opts = new ExpandoObject();
            if (url != null)
				this.opts.url = url;
        }

        string script
        {
            get;
            set;
        }

        dynamic opts
        {
            get;
            set;
        }

        dynamic handler
        {
            get;
            set;
        }

        async void start(string url = null)
        {
            if (url != null) this.opts.url = url;

            var start = Edge.Func(this.script);

            await Task.Run(async () =>
            {
                this.handler = await start(opts);
            });
        }

        public void on(string name, Func<object, Task<object>> callback)
        {
            if (this.handler != null)
            {
                this.handler.on(new { name = name, callback = callback });
                return;
            }
            
            ((IDictionary<string, object>)this.opts).Add(name, callback);
        }

        public void emit(string name, object data)
        {
            if (this.handler == null) return;
            this.handler.emit(new { name = name, data = data });
        }

        public void disconnect()
        {
            if (this.handler == null) return;
            this.handler.disconnect();
        }

        public void forceDisconnect()
        {
            if (this.handler == null) return;
            this.handler.forceDisconnect();
        }

    }
}
