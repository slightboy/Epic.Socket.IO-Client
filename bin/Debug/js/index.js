module.exports = function (opts, callback) {


var socket = require('socket.io-client')(opts.url || opts, opts.opts || {});
  socket.on('connect', function(){

    for(var key in opts)
    {
      if (key != "url" && key != "opts")
        socket.on(key, opts[key])
    }


    socket.on('event', function(data){});
    socket.on('disconnect', function(){});
  });


  var result = {};
  result.on = function(data, callback) { socket.on(data.name, data.callback); }
  result.emit = function(protocol, callback) { socket.emit(protocol.name, protocol.data); }
  result.disconnect = function(data, callback) { socket.emit('disconnect'); }
  result.forceDisconnect = function(data, callback) { socket.emit('forceDisconnect'); }

  callback(null, result);

};