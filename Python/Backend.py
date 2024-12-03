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

from flasgger import Swagger, swag_from
from flask import Flask, jsonify, request
from sqlalchemy import create_engine, select
from sqlalchemy.orm import sessionmaker
from sqlalchemy_serializer import Serializer

import Tables as Tab

# API-setup
app = Flask("MantaDiveBackend")
Swagger(app)

# DB-setup
engine = create_engine("sqlite:///./dev.db")

Session = sessionmaker(bind=engine)
session = Session()


@swag_from("./swagger/getAllUsers.yml")
@app.route("/api/getAllUsers", methods=["GET"])
def getUsers():
    users = session.query(Tab.User).all()

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
    seeds = session.query(Tab.Seed).all()

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
        return jsonify(f"error: Seed for date-{UUID} does not exist")


if __name__ == "__main__":
    app.run(debug=True,host='0.0.0.0')

    session.close()
