import json
import DateTime

from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

import Tables as Tab

if __name__ == "__main__":

    engine = create_engine("sqlite:///./dev.db")
    Session = sessionmaker(bind=engine)
    session = Session()

    Tab.Base.metadata.create_all(engine, checkfirst=True)

    # Reset the database
    meta = Tab.Base.metadata
    for table in reversed(meta.sorted_tables):
        session.execute(table.delete())
    session.commit()

    print("-----ADDING USERS-----")
    with open("./TestDbData/Users.json", "r") as f:
        users = json.load(f)

        for user in users:
            print(user)
            newUser = Tab.User(
                UUID=user["UUID"],
                UserName=user["UserName"],
                Tier=user["Tier"],
                MaxDepth=user["MaxDepth"],
                DailyDepth=user["DailyDepth"],
                Upgrades=user["Upgrades"],
                Currency=user["Currency"],
            )

            session.add(newUser)

    print("-----ADDING SEEDS-----")
    with open("./TestDbData/Seeds.json", "r") as f:
        seeds = json.load(f)

        for seed in seeds:
            print(seed)
            newSeed = Tab.Seed(
                ID = 
                Date = seed["Date"],
                Value = seed["Seed"]
            )

            session.add(newSeed)

    session.commit()
    session.close()
