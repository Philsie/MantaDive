"""
MantaDive Backend Application
------------------------------
This is the Flask backend for the MantaDive project. It handles API endpoints,
database interactions, and other server-side functionalities for the platform.

Repository: https://github.com/Philsie/MantaDive
Branch: python-backend_and_DB

Author: Philsie
Date: 26/11/2024
"""

import io
from random import randint

import matplotlib.pyplot as plt
from apscheduler.schedulers.background import BackgroundScheduler
from DateTime import DateTime

# %% Imports
from flasgger import Swagger, swag_from
from flask import Flask, Response, jsonify, request
from sqlalchemy import create_engine, func
from sqlalchemy.exc import IntegrityError
from sqlalchemy.orm import sessionmaker
from sqlalchemy.orm.exc import NoResultFound
from sqlalchemy_serializer import Serializer

import Tables as Tab

# %% Setup
# API-setup
app = Flask("MantaDiveBackend")
Swagger(app)

# DB-setup
engine = create_engine("sqlite:///./dev.db")

Session = sessionmaker(bind=engine)
session = Session()

#%% Scheduled Tasks
def generateDailySeed():
    with app.app_context():
        print("*")
        print(newSeed())


#%% Routes
@swag_from("./swagger/getAllUsers.yml")
@app.route("/api/getAllUsers", methods=["GET"])
def getUsers():
    users = session.query(Tab.User).order_by(Tab.User.UUID.asc()).all()

    users = [user.__export__() for user in users]

    return jsonify(users)


@swag_from("./swagger/getLeaderboard.yml")
@app.route("/api/getLeaderboard/<int:LeaderboardSpots>", methods=["GET"])
def getLeaderboard(LeaderboardSpots):
    users = session.query(Tab.User).order_by(Tab.User.MaxDepth.desc()).limit(LeaderboardSpots)

    users = [{"UserName":user.UserName,"MaxDepth":user.MaxDepth} for user in users]

    return jsonify(users)

@swag_from("./swagger/getDailyLeaderboard.yml")
@app.route("/api/getDailyLeaderboard/<int:LeaderboardSpots>", methods=["GET"])
def getDailyLeaderboard(LeaderboardSpots):
    users = (
        session.query(Tab.User)
        .order_by(Tab.User.DailyDepth.desc())
        .limit(LeaderboardSpots)
    )

    users = [
        {"UserName": user.UserName, "DailyDepth": user.DailyDepth} for user in users
    ]

    return jsonify(users)


@swag_from("./swagger/resetDailyLeaderboard.yml")
@app.route("/api/resetDailyLeaderboard", methods=["PATCH"])
def resetDailyLeaderboard():
    session.query(Tab.User).update({"DailyDepth": 0})
    session.commit
    return getDailyLeaderboard(5)

@app.route("/api/user/<UUID>", methods=["GET", "PATCH"])
@swag_from("./swagger/user.yml")
def user(UUID):
    user = session.query(Tab.User).filter(Tab.User.UUID == UUID).first()
    if user:
        if request.method == "GET":
            return jsonify(user.__export__())
        elif request.method == "PATCH":
            args = request.args
            for key in list(args.keys()):
                try:
                    res, reply = user.setParam(key, args[key])
                    if res == False:
                        return jsonify(reply)
                except ValueError:
                    return jsonify(
                        f"error: user with uuid-{UUID} has no attribute-{key}"
                    )
            session.commit()
            return jsonify(user.__export__())
    else:
        return jsonify(f"error: user with uuid-{UUID} does not exist")


@app.route("/api/userUpgrades/<UUID>", methods=["GET", "PATCH"])
@swag_from("./swagger/userUpgrades.yml")
def userUpgrades(UUID):
    user = session.query(Tab.User).filter(Tab.User.UUID == UUID).first()
    if user:
        if request.method == "GET":
            return jsonify(
                user.__export__(
                    uuid=True,
                    UserName=True,
                    Tier=False,
                    MaxDepth=False,
                    DailyDepth=False,
                    Upgrades=True,
                    Currency=False,
                )
            )
        elif request.method == "PATCH":
            args = request.args
            for key in list(args.keys()):
                try:
                    res, reply = user.setUpgrades(key, args[key])
                    if not res:
                        return jsonify(reply)
                except ValueError:
                    return jsonify(
                        f"error: user with uuid-{UUID} has no attribute-{key}"
                    )

            session.commit()
            session.refresh(user)

            return jsonify(user.__export__(
                    uuid=True,
                    UserName=True,
                    Tier=False,
                    MaxDepth=False,
                    DailyDepth=False,
                    Upgrades=True,
                    Currency=False,
                ))  # Return updated values
    else:
        return jsonify(f"error: user with uuid-{UUID} does not exist")

@app.route("/api/userCurrencies/<UUID>", methods=["GET", "PATCH"])
@swag_from("./swagger/userCurrencies.yml")
def userCurrencies(UUID):
    user = session.query(Tab.User).filter(Tab.User.UUID == UUID).first()
    if user:
        if request.method == "GET":
            return jsonify(
                user.__export__(
                    uuid=True,
                    UserName=True,
                    Tier=False,
                    MaxDepth=False,
                    DailyDepth=False,
                    Upgrades=False,
                    Currency=True,
                )
            )
        elif request.method == "PATCH":
            args = request.args
            for key in list(args.keys()):
                try:
                    res, reply = user.setCurrencies(key, args[key])
                    if not res:
                        return jsonify(reply)
                except ValueError:
                    return jsonify(
                        f"error: user with uuid-{UUID} has no attribute-{key}"
                    )

            session.commit()
            session.refresh(user)

            return jsonify(user.__export__(
                    uuid=True,
                    UserName=True,
                    Tier=False,
                    MaxDepth=False,
                    DailyDepth=False,
                    Upgrades=False,
                    Currency=True,
                ))  # Return updated values
    else:
        return jsonify(f"error: user with uuid-{UUID} does not exist")

@swag_from("./swagger/getAllSeeds.yml")
@app.route("/api/getAllSeeds", methods=["GET"])
def getAllSeeds():
    seeds = session.query(Tab.Seed).order_by(Tab.Seed.ID.asc()).all()

    seeds = [seed.__export__() for seed in seeds]

    return jsonify(seeds)

@swag_from("./swagger/getSeed.yml")
@app.route("/api/getSeed/<date>", methods=["GET"])
def getSeed(date):
    print(date)
    y,m,d = str(date).split("_")
    seed = session.query(Tab.Seed).filter(Tab.Seed.Date["Year"] == y,Tab.Seed.Date["Month"] == m,Tab.Seed.Date["Day"] == d).first()
    if seed:
        return jsonify(seed.__export__())
    else:
        return jsonify(f"error: Seed for date-{date} does not exist")

#@swag_from("./swagger/newSeed.yml")
@app.route("/api/newSeed", methods=["GET","PUT"])
def newSeed():
    date = DateTime().parts()

    latestDailySeed = session.query(Tab.Seed).order_by(Tab.Seed.Date["Year"].desc(),Tab.Seed.Date["Month"].desc(),Tab.Seed.Date["Day"].desc()).first()


    print("---------")
    print(latestDailySeed.Date)
    print(date)
    print("---------")
    if latestDailySeed.Date["Year"] == date[0] and latestDailySeed.Date["Month"] == date[1] and latestDailySeed.Date["Day"] == date[2]:
        return jsonify(f"Seed for date-{DateTime().Date()} already exists")

    seed_value = randint(0,65_536)

    while session.query(Tab.Seed).filter(Tab.Seed.Value == seed_value).first():
        seed_value = randint(0,65_536)

    newSeed = Tab.Seed(
        Date = {
            "Year": date[0],
            "Month": date[1],
            "Day": date[2]},
        Value = seed_value
        )

    Tab.attributes.flag_modified(newSeed, "Value")
    session.add(newSeed)

    session.commit()
    session.refresh(newSeed)
    return jsonify(f"{date[0]}_{date[1]}_{date[2]}")
    
@swag_from("./swagger/getAllShopItems.yml")
@app.route("/api/getAllShopItems", methods=["GET"])
def getAllShopItems():
    shopItems = session.query(Tab.ShopItem).order_by(Tab.ShopItem.ID.asc()).all()

    shopItems = [shopItem.__export__() for shopItem in shopItems]

    return jsonify(shopItems)

@swag_from("./swagger/getShopItem.yml")
@app.route("/api/getShopItem/<ID>", methods=["GET"])
def getShopItem(ID):
    shopItem = session.query(Tab.ShopItem).filter(Tab.ShopItem.ID == ID).first()
    if shopItem:
        return jsonify(shopItem.__export__())
    else:
        return jsonify(f"error: ShopItem with ID-{ID} does not exist")

@swag_from("./swagger/getAvailableShopItems-ByUser.yml")
@app.route("/api/getAvailableShopItems/<UUID>", methods=["GET"])
def getAvailableShopItems(UUID):
    user = session.query(Tab.User).filter(Tab.User.UUID == UUID).first()
    res = []
    if user:
        shopItems = session.query(Tab.ShopItem).order_by(Tab.ShopItem.ID.asc()).all()

        lockedUpgrades = user.ShopItems_Locked.split("_")
        boughtUpgrades = user.ShopItems_Bought.split("_")
        availableUpgrades = [str(shopItem.ID) for shopItem in shopItems]

        # remove owned or locked upgrades
        setLockedUpgrade = set(lockedUpgrades)
        setBoughtUpgrades = set(boughtUpgrades)
        setAvailableUpgrade = set(availableUpgrades)

        setAvailableUpgrade = setAvailableUpgrade.difference(setLockedUpgrade).difference(setBoughtUpgrades)
        availableUpgrades = list(setAvailableUpgrade)

        availableUpgrades = sorted(availableUpgrades,key=int)

        # check if PreReq are fullfilled
        for shopItemID in availableUpgrades:
            shopItem = session.query(Tab.ShopItem).filter(Tab.ShopItem.ID == int(shopItemID)).first()
            if shopItem.PreReq == "":
                res.append(shopItem.__export__())
            else:
                if all(element in boughtUpgrades for element in shopItem.PreReq.split("_")):
                    res.append(shopItem.__export__())

        return jsonify(res)
    else:
        return jsonify(f"error: user with uuid-{UUID} does not exist")

@swag_from("./swagger/unlockShopItem-ByUser-ByShopItem.yml")
@app.route("/api/unlockShopItem/<UUID>/<ShopItemID>", methods=["GET","PATCH"])
def unlockShopItem(UUID, ShopItemID):
    user = session.query(Tab.User).filter(Tab.User.UUID == UUID).first()
    if user:
        if str(ShopItemID) in user.ShopItems_Bought.split("_") or str(
            ShopItemID
        ) in user.ShopItems_Locked.split("_"):
            return jsonify(
                f"error: User with uuid-{UUID} has ShopItem with id-{ShopItemID} already purchased or is locked out"
            )
        shopItem = (
            session.query(Tab.ShopItem).filter(Tab.ShopItem.ID == ShopItemID).first()
        )
        if shopItem:
            # check what currerency to use and if user has sufficient amount
            currecyUsed = ""
            if shopItem.Price["Standard"] > 0:
                currecyUsed = "Standard"
            elif shopItem.Price["Premium"] > 0:
                currecyUsed = "Premium"
            else:
                return jsonify(
                    f"error: ShopItem with id-{ShopItemID} has no/wrong price set"
                )

            if user.Currency[currecyUsed] < shopItem.Price[currecyUsed]:
                return jsonify(
                    f"error: User with uuid-{UUID} has has insufficient Currency to purchase ShopItem with id-{ShopItemID}"
                )

            # update bought items
            newBought = user.ShopItems_Bought
            newBought += "_"+str(shopItem.ID)
            user.ShopItems_Bought = newBought

            # update locked items
            if shopItem.Locks != "":
                newLocked = user.ShopItems_Locked
                newLocked += "_"+str(shopItem.Locks)
                user.ShopItems_Locked = newLocked

            # take currency from user
            newCurrencyAmount = user.Currency[currecyUsed] - shopItem.Price[currecyUsed]
            user.setCurrencies(currecyUsed, newCurrencyAmount)

            return jsonify(user.__export__())
        else:
            return jsonify(f"error: ShopItem with id-{ShopItemID} does not exist")
    else:
        return jsonify(f"error: user with uuid-{UUID} does not exist")
        
@swag_from("./swagger/post_levelMetadata.yml")
@app.route("/api/levelMetadata/<UUID>", methods=["POST"])
def postLevelMetadata(UUID):
    try:
        data = request.get_json()
        if not data:
            return jsonify({"error": "Invalid JSON payload"}), 400

        time_elapsed = data.get('TimeElapsed')
        shots_fired = data.get('ShotsFired')
        enemies_hit = data.get('EnemiesHit')
        coins_collected = data.get('CoinsCollected')

        if None in (time_elapsed, shots_fired, enemies_hit, coins_collected):
            return jsonify({"error": "Missing required fields"}), 400

        newLevelMetadata = Tab.levelMetadata(
            player_id=UUID,
            time_elapsed=time_elapsed,
            shots_fired=shots_fired,
            enemies_hit=enemies_hit,
            coins_collected=coins_collected,
        )
        session.add(newLevelMetadata)
        session.commit()

        return jsonify({"message": "Game data added successfully"}), 201

    except Exception as e:
        print(f"Error: {e}")  # Good for debugging
        return jsonify({"error": "An error occurred"}), 500

@swag_from("./swagger/get_levelMetadata.yml")
@app.route("/api/levelMetadata/<levelMetadataId>", methods=["GET"])
def getLevelMetadata(levelMetadataId):
    try:
        level_data = session.query(Tab.levelMetadata).filter_by(
            id=levelMetadataId).one()

        result = {
            'id': level_data.id,
            'player_id': level_data.player_id,
            'time_elapsed': level_data.time_elapsed,
            'shots_fired': level_data.shots_fired,
            'enemies_hit': level_data.enemies_hit,
            'coins_collected': level_data.coins_collected,
        }

        return jsonify(result), 200

    except NoResultFound:
        return jsonify({"error": "Entry not found"}), 404

    except Exception as e:
        print(f"Error: {e}")
        return jsonify({"error": "An error occurred"}), 500


@app.route("/stats", methods=["GET"])
def get_statistics():

    # Aggregate statistics for the last four fields using the Tab alias for levelMetadata
    stats = session.query(
        func.avg(Tab.levelMetadata.time_elapsed).label("avg_time"),
        func.avg(Tab.levelMetadata.shots_fired).label("avg_shots"),
        func.avg(Tab.levelMetadata.enemies_hit).label("avg_hits"),
        func.avg(Tab.levelMetadata.coins_collected).label("avg_coins"),
    ).one()

    session.close()

    # Handle None values by replacing them with 0 (or another appropriate value)
    stats_dict = {
        "avg_time": stats.avg_time if stats.avg_time is not None else 0,
        "avg_shots": stats.avg_shots if stats.avg_shots is not None else 0,
        "avg_hits": stats.avg_hits if stats.avg_hits is not None else 0,
        "avg_coins": stats.avg_coins if stats.avg_coins is not None else 0,
    }

    labels = ["Time Elapsed", "Shots Fired", "Enemies Hit", "Coins Collected"]
    values = [
        stats_dict["avg_time"],
        stats_dict["avg_shots"],
        stats_dict["avg_hits"],
        stats_dict["avg_coins"],
    ]

    # Create subplots for 4 different charts
    fig, axes = plt.subplots(2, 2, figsize=(10, 8))

    colors = ["blue", "red", "green", "orange"]

    for i, ax in enumerate(axes.flat):
        ax.bar([labels[i]], [values[i]], color=colors[i])
        ax.set_title(labels[i])
        ax.set_ylabel("Average Value")

    plt.tight_layout()

    # Convert plot to image
    img_io = io.BytesIO()
    plt.savefig(img_io, format="png")
    img_io.seek(0)

    return Response(img_io.getvalue(), mimetype="image/png")


# %% on run
if __name__ == "__main__":
    #%% Start Scheduled Task
    scheduler = BackgroundScheduler()
    scheduler.add_job(generateDailySeed, 'interval', minutes=1) # Checks 1x a minute
    #scheduler.add_job(generateDailySeed, 'cron', hour=0, minute=0)  # Runs at midnight
    scheduler.start()

    #%% Start Server
    app.run(debug=True,host='0.0.0.0')

    session.close()
