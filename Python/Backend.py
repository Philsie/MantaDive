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


@app.route("/api/user/<UUID>", methods=["GET", "POST"])
@swag_from("./swagger/user.yml")
def user(UUID):
    user = session.query(Tab.User).filter(Tab.User.UUID == UUID).first()
    if user:
        if request.method == "GET":
            return jsonify(user.__export__())
        elif request.method == "POST":
            args = request.args
            print(list(args.keys()))
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

if __name__ == "__main__":
    app.run(debug=True)

    session.close()
