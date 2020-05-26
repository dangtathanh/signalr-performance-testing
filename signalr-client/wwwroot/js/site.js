var data = {
    numConnections: 0,
    numMessageSent: 0,
    numMessageReceived: 0
}

var $tblMsg = $('#tbl-messages');
var $rowMsgTemplate = $tblMsg.find('.msg-item.d-none');
var $btnConnect = $('.btn-primary');
var $btnSend = $('.btn-success.btn-sent');

function connect() {
    $btnConnect.attr("disabled", "disabled");
    var i;
    for (i = 0; i < 200; i++) {
        var chat = new chathub();
        chat.start();
    };
}

function sendMessages() {
    console.log($(this));
    $btnSend.attr("disabled", "disabled").html("Sending...");
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "http://localhost:5000/api/v1/performance-testing/start",
        "method": "POST",
        "headers": {
            "cache-control": "no-cache",
            "postman-token": "665f1ab5-194f-0651-885c-e4a4914fcdd8"
        }
    }

    $.ajax(settings).done(function (response) {
        $btnSend.removeAttr("disabled").html("Send messages");
    });
}

function getResult() {
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "http://localhost:5000/api/v1/performance-testing/result",
        "method": "GET",
        "headers": {
            "cache-control": "no-cache",
            "postman-token": "4ab64b96-d71f-d894-7f34-d69d4758c1cc"
        }
    }

    $.ajax(settings).done(function (response) {
        alert(`Server received ${response.numMsgReceived} replies from ${response.numConnections} clients`);
    });
}


function updateNumConnections() {
    $('#num-connected').text(data.numConnections);
}

function updateNumMessagesReceived() {
    $('#num-messages-sent').text(data.numMessageReceived);
}

function updateNumMessagesSent() {
    $('#num-messages-received').text(data.numMessageSent);
}

function chathub() {
    var _connectionId = "";

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5000/chathub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
            console.log("connected");
        } catch (err) {
            console.log(err);
            setTimeout(() => start(), 5000);
        }
    };

    connection.onclose(async () => {
        await start();
    });

    connection.on("Connected", (connectionId, numConnections) => {
        console.log("Connected: ", connectionId, numConnections);
        _connectionId = connectionId;
        data.numConnections = numConnections;
        updateNumConnections();
    });

    connection.on("NewMessage", (sender, message) => {
        console.log("Received new messages: ", sender, message);
        data.numMessageReceived++;
        updateNumMessagesReceived();
        reply(message);
        appendMessage(sender, _connectionId, message);
    });

    function reply(message) {
        message = `Reply ${message}`;
        connection.invoke("SendMessage", _connectionId, message)
            .then(() => {
                data.numMessageSent++;
                updateNumMessagesSent();
                appendMessage(_connectionId, '', message)
            }).catch(err =>
                console.error(err)
            );
    }

    async function appendMessage(sender, receiver, message) {
        var $row = $rowMsgTemplate.clone().removeClass('d-none');
        $row.find('.msg-item-sender').text(sender);
        $row.find('.msg-item-receiver').text(receiver);
        $row.find('.msg-item-messages').text(message);
        $row.find('.msg-item-received').text((new Date()).toUTCString());
        $tblMsg.prepend($row);
    }

    return {
        start: start
    }
}