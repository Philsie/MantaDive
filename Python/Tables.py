from sqlalchemy import JSON, Column, Float, Integer, Sequence, String, cast
from sqlalchemy.orm import attributes, declarative_base
from random import randint

Base = declarative_base()


class User(Base):
    __tablename__ = "User"

    UUID = Column(Integer, Sequence("user_uuid_seq"), primary_key=True)
    UserName = Column(String(255))
    Tier = Column(Integer)
    MaxDepth = Column(Float)
    DailyDepth = Column(Float)
    Upgrades = Column(JSON)
    Currency = Column(JSON)

    def __repr__(self):
        return f"<User(UUID={self.UUID}, UserName='{self.UserName}', Tier={self.Tier}, MaxDepth={self.MaxDepth}, DailyDepth={self.DailyDepth}, Upgrades={self.Upgrades}, Currency={self.Currency})>"

    def __export__(self, uuid=True, UserName=True, Tier=True, MaxDepth=True, DailyDepth=True, Upgrades=True, Currency=True):
        return {
            key: getattr(self, key)
            for key, include in {
                "UUID": uuid,
                "UserName": UserName,
                "Tier": Tier,
                "MaxDepth": MaxDepth,
                "DailyDepth": DailyDepth,
                "Upgrades": Upgrades,
                "Currency": Currency
            }.items()
            if include
        }

    def setParam(self, key, value):
        if key == "UUID":
            return False, "Cant change UUID"
        if key == "Upgrades":
            return False, "Use setUpgrades() to change Upgrade status"
        else:
            setattr(self, key, value)
            return True, ""

    def setUpgrades(self, key, value):
        if key in self.Upgrades:
            self.Upgrades[key] = value
            attributes.flag_modified(self, "Upgrades")
            return True, ""
        else:
            return (
                False,
                f"Wrong key provided during setUpgrades(\\n\\t{self.__export__},\\n\\t{key},\\n\\t{value}\\n)",
            )

    def setCurrencies(self, key, value):
        if key in self.Currency:
            self.Currency[key] = value
            attributes.flag_modified(self, "Currency")
            return True, ""
        else:
            return (
                False,
                f"Wrong key provided during setCurrencies(\\n\\t{self.__export__},\\n\\t{key},\\n\\t{value}\\n)",
            )



class Seed(Base):
    __tablename__ = "Seed"

    Date = Column(String, primary_key=True)
    Seed = Column(Integer, unique=True)  # Enforce uniqueness for the Seed column

    def __init__(self, date, seed=None):
        self.Date = date
        if seed is None:
            self.Seed = self._generate_unique_seed()  # Generate a unique random integer
        else: self.Seed = seed

    def _generate_unique_seed(self):
        """Generates a unique random integer for the Seed column, handling potential conflicts."""
        while True:
            seed = random.randint(0, 65535)
            try:
                # Attempt to add the new entry with the generated seed
                session.add(Seed(date='2024-12-03', Seed=seed))  # Replace with your session object
                session.commit()  # Commit the transaction
                return seed  # Return the generated seed if successful
            except IntegrityError:
                # If a uniqueness violation occurs, retry generating a new seed
                session.rollback()  # Rollback the transaction
                continue

    def __export__(self):
        return {
            "Date": {self.Date},
            "Seed": {self.Seed}
        }