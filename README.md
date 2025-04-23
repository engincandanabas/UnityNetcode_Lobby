# 🎮 Unity Multiplayer Lobby System with PlayFab, Netcode, and CloudScript

This project is a fully functional multiplayer lobby system built using **Unity**, **Netcode for GameObjects**, **Unity Lobby**, **Relay**, and **PlayFab CloudScript**. It features a seamless authentication flow, lobby creation & browsing, real-time player visibility, and unique username enforcement.

---

## 🚀 Features

- ✅ **Anonymous Authentication** with Unity Authentication
- 🧾 **Unique Username Reservation** using PlayFab CloudScript
- 🏠 **Lobby Creation** (UI for name, visibility, and max player count)
- 🧍🧍‍♂️ **Join Lobby** and see other connected players
- 🌐 **Lobby Browser** with real-time updates
- 🔐 **Host/Client Management** with Relay
- 🕹️ **Game Scene Switching** when lobby is full (auto start)
- ♻️ **Username is released** on player exit or disconnect

---

## 🧠 Username Reservation with PlayFab

We use **PlayFab TitleData** and **CloudScript** to reserve usernames globally.

> ❗ Username checking is handled via PlayFab CloudScript because Netcode (and Lobby) only track players **currently connected**. If a player disconnects or closes the game, their username would not be tracked anymore unless a global, persistent backend like PlayFab is used.

### ✅ Why not check usernames inside Netcode or Lobby?
- Lobby player data is only available **while players are inside the lobby**.
- If a user reserves a username and exits **before joining a lobby**, Netcode or Lobby won't track it.
- PlayFab CloudScript ensures **global and persistent username uniqueness** across all sessions.

---

## 🧪 Setup Instructions

### 🔧 Prerequisites
- Unity 2022+ (or compatible version with UGS)
- Unity Services (Authentication, Lobby, Relay) enabled
- PlayFab Title setup

### 🔑 Unity Services Setup
1. Enable Unity Authentication, Lobby, and Relay from **Dashboard**.
2. Link your Unity Project to a Unity Cloud Project ID.

### 🔗 PlayFab Setup
1. Create a PlayFab Title at [PlayFab Dashboard](https://developer.playfab.com)
2. Go to **Automation > CloudScript**
3. Add the following CloudScript function:

```javascript
handlers.CheckUsernameAvailability = function (args) {
    var username = args.username;

    var titleData = server.GetTitleData({ Keys: ["UsedUsernames"] });
    var usedList = [];

    if (titleData.Data && titleData.Data["UsedUsernames"]) {
        usedList = JSON.parse(titleData.Data["UsedUsernames"]);
    }

    var isTaken = usedList.includes(username);

    if (!isTaken) {
        usedList.push(username);
        server.SetTitleData({
            Key: "UsedUsernames",
            Value: JSON.stringify(usedList)
        });
    }

    return { isTaken: isTaken };
};

handlers.ReleaseUsername = function (args) {
    var username = args.username;
    var titleData = server.GetTitleData({ Keys: ["UsedUsernames"] });

    if (titleData.Data && titleData.Data["UsedUsernames"]) {
        var usedList = JSON.parse(titleData.Data["UsedUsernames"]);
        var index = usedList.indexOf(username);
        if (index > -1) {
            usedList.splice(index, 1);
            server.SetTitleData({
                Key: "UsedUsernames",
                Value: JSON.stringify(usedList)
            });

        }
    }

    return { success: true };
};

<img src="https://github.com/user-attachments/assets/36d8633c-4564-4a2a-847e-b2bbbd1d9d40"  width="400" height="400" >

![login2](https://github.com/user-attachments/assets/9fca22ea-9096-4c20-aaff-75308a56a19b)

![lobbyList](https://github.com/user-attachments/assets/5f840926-c49a-48cf-b8d4-aa99a5a7bf28)

![createLobby](https://github.com/user-attachments/assets/d8ed7dc8-1cde-473f-a20e-fd9cd6236bd7)

![lobbyUI](https://github.com/user-attachments/assets/9c9f5e40-5f02-4863-abcf-25e0b6e7e344)



