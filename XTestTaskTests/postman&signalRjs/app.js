//postman not working with signalr, so i create this to test it

var signalR = require('@microsoft/signalr');
const { func } = require('prop-types');

const accountCon = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/hubs/account")
    .configureLogging(signalR.LogLevel.Information)
    .build();

const chatCon = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/hubs/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

accountCon.onclose(async () => {
    console.log("a SignalR Disconnected.");
    await startAccountCon();
});

chatCon.onclose(async () => {
    console.log("c SignalR Disconnected.");
    await startChatCon();
});

chatCon.on('CloseChat', chatId => {
    console.log("closeChat: " + chatId);
});

chatCon.on('ReceiveMessage', message => {
    console.log("receiveMessage: " + JSON.stringify(message));
});

chatCon.on('DeleteMessage', msgId => {
    console.log("deleteMessage: " + msgId);
});

chatCon.on('UpdateMessage', message => {
    console.log("updateMessage: " + JSON.stringify(message));
});

chatCon.on('UpdateChat', chat => {
    console.log("UpdateChat: " + JSON.stringify(chat));
});

async function startAccountCon() {
    try {
        await accountCon.start();
        getAccounts();
        createAccount('name');
        console.log("a SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(startAccountCon, 5000);
    }
};

async function startChatCon() {
    try {
        await chatCon.start();
        console.log("c SignalR Connected.");
        let userId = 3;
        let chatId = 5;
        let messageId = 13;
        let name = "name";
        let message = "message";
        let offset = 0;
        let limit = 100;

        //await connect(userId); //v
        await joinChat(userId, chatId); //v
        //await createChat(userId, name); //v
        //await sendMessage(userId, chatId, message); //v
        //await getChats(offset, limit); //v
        //await getChat(chatId); //v
        await getChatMessages(chatId, offset, limit); //v
        //await getChatMembers(chatId, offset, limit); //
        //await updateChat(chatId, userId, name); //v
        //await updateMessage(messageId, userId, message); //v
        //await deleteChat(chatId, userId); //v
        await deleteMessage(chatId, messageId, userId); //v
        
    } catch (err) {
        console.log(err);
        setTimeout(startChatCon, 5000);
    }
};

//startAccountCon();
startChatCon();

async function getAccounts() {
    let page = {offest: 0 ,limit: 100}
    accountCon.invoke("GetAccounts", page).then(
        data => {console.log(data);}
    );
}

async function createAccount(name) {
    let account = {
        name: name
    }
    accountCon.invoke("CreateAccount", account).then(
        data => {console.log(data);}
    );
}


async function connect(userId) {
    chatCon.invoke("Connect", userId).then(
        data => {console.log(data);}
    );
}

async function joinChat(userId, chatId) {
    chatCon.invoke("JoinChat", userId, chatId).then(
        data => {console.log(data);}
    );
}

async function createChat(userId, name) {
    let chat = {
        userId: userId,
        name: name
    }
    chatCon.invoke("CreateChat",  chat).then(
        data => {console.log(data);}
    )
    .catch(err => {console.log('err: ')});
}

async function sendMessage(userId, chatId, message) 
{
    let msg = {
        userId: userId,
        message: message
    }
    chatCon.invoke("SendMessage", userId, chatId, msg).then(
        data => {console.log(data);}
    );
}

async function getChats(offset, limit) {
    let page = {
        offset: offset,
        limit: limit
    }
    chatCon.invoke("GetChats", page).then(
        data => {console.log(data);}
    );
}

async function getChat(chatId) {
    chatCon.invoke("GetChat", chatId).then(
        data => {console.log(data);}
    );
}

async function getChatMessages(chatId, offset, limit) {
    let page = {
        offset: offset,
        limit: limit
    }
    chatCon.invoke("GetChatMessages", chatId, page).then(
        data => {console.log(data);}
    );
}

async function getChatMembers(chatId, offset, limit) {
    let page = {
        offset: offset,
        limit: limit
    }
    chatCon.invoke("GetChatMembers", chatId, page).then(
        data => {console.log(data);}
    );
}

async function updateChat(chatId, userId, name) {
    let chat = {
        userId: userId,
        name: name
    }
    chatCon.invoke("UpdateChat", chatId, chat).then(
        data => {console.log(data);}
    );
}

async function updateMessage(messageId, userId, message) {
    let msg = {
        userId: userId,
        message: message
    }
    chatCon.invoke("UpdateMessage", messageId, msg).then(
        data => {console.log(data);}
    );
}

async function deleteChat(chatId, userId) {
    try {

        chatCon.invoke("DeleteChat", chatId, userId)
        .then(
            data => {console.log(data);}
        )
        .catch(err => {console.log('err: ')})
    } catch(err) {console.log('err:', err);}
    
}

async function deleteMessage(chatId, messageId, userId) {
    chatCon.invoke("DeleteMessage", chatId, messageId, userId).then(
        data => {console.log(data);}
    )
    .catch(err => {console.log('err: ')});
}