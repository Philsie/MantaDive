from flask import Flask, jsonify
from sqlalchemy import create_engine, select
from sqlalchemy.orm import sessionmaker
from sqlalchemy_serializer import Serializer

import Tables as Tab

# API-setup
app = Flask("MantaDiveBackend")

# DB-setup
engine = create_engine("sqlite:///./dev.db")

Session = sessionmaker(bind=engine)
session = Session()


@app.route("/api/getAllUsers")
def getUsers():
    users = session.query(Tab.User).all()

    users = [user.__export__() for user in users]

    return jsonify(users)

@app.route("/api/getLeaderboard/<int:LeaderboardSpots>")
def getLeaderboard(LeaderboardSpots):
    users = session.query(Tab.User).order_by(Tab.User.MaxDepth.desc()).limit(LeaderboardSpots)

    users = [{"UserName":user.UserName,"MaxDepth":user.MaxDepth} for user in users]

    return jsonify(users)


if __name__ == "__main__":
    app.run(debug=True)

    # print(str(session.query(Tab.User).all()))
    session.close()
