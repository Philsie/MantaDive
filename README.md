# API Endpoints

## User Management
- **GET `/api/getAllUsers`**: Retrieve all users.
- **GET `/api/user/<UUID>`**: Retrieve or update a specific user.
- **PATCH `/api/userUpgrades/<UUID>`**: Update user upgrades.
- **PATCH `/api/userCurrencies/<UUID>`**: Update user currencies.

## Leaderboards
- **GET `/api/getLeaderboard/<int:LeaderboardSpots>`**: Retrieve the leaderboard.
- **GET `/api/getDailyLeaderboard/<int:LeaderboardSpots>`**: Retrieve the daily leaderboard.
- **PATCH `/api/resetDailyLeaderboard`**: Reset the daily leaderboard.

## Shop Items
- **GET `/api/getAllShopItems`**: Retrieve all shop items.
- **GET `/api/getShopItem/<ID>`**: Retrieve a specific shop item.
- **GET `/api/getAvailableShopItems/<UUID>`**: Retrieve available shop items for a user.
- **PATCH `/api/unlockShopItem/<UUID>/<ShopItemID>`**: Unlock a shop item for a user.

## Game Metadata
- **POST `/api/levelMetadata/<UUID>`**: Post level metadata.
- **GET `/api/levelMetadata/<levelMetadataId>`**: Retrieve level metadata.

## Statistics
- **GET `/stats`**: Generate and display game statistics.

# Database Schema

## User Table
- **UUID**: Primary key, auto-incremented.
- **UserName**: String, max length 255.
- **Tier**: Integer.
- **MaxDepth**: Float.
- **DailyDepth**: Float.
- **Upgrades**: JSON.
- **Currency**: JSON.
- **ShopItems_Bought**: String.
- **ShopItems_Locked**: String.

## Seed Table
- **ID**: Primary key, auto-incremented.
- **Date**: JSON, unique.
- **Value**: Integer, unique.

## ShopItem Table
- **ID**: Primary key, auto-incremented.
- **Name**: String.
- **Description**: String.
- **PreReq**: String.
- **Locks**: String.
- **Price**: JSON.
- **Effect**: JSON.
- **Sprite**: String.

## levelMetadata Table
- **id**: Primary key, auto-incremented.
- **player_id**: Integer, not null.
- **time_elapsed**: Float, not null.
- **shots_fired**: Integer, not null.
- **enemies_hit**: Integer, not null.
- **coins_collected**: Integer, not null.
