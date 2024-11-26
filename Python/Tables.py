from sqlalchemy import JSON, Column, Float, Integer, Sequence, String, cast
from sqlalchemy.orm import attributes, declarative_base

Base = declarative_base()


class User(Base):
    __tablename__ = "User"

    UUID = Column(Integer, Sequence("user_uuid_seq"), primary_key=True)
    UserName = Column(String(255))
    Tier = Column(Integer)
    MaxDepth = Column(Float)
    DailyDepth = Column(Float)
    Upgrades = Column(JSON)

    def __repr__(self):
        return f"<User(UUID={self.UUID}, UserName='{self.UserName}', Tier={self.Tier}, MaxDepth={self.MaxDepth}, DailyDepth={self.DailyDepth}, Upgrades={self.Upgrades})>"

    def __export__(self, uuid=True, UserName=True, Tier=True, MaxDepth=True, dailyDepth=True, Upgrades=True):
        return {
            key: getattr(self, key)
            for key, include in {
                "UUID": uuid,
                "UserName": UserName,
                "Tier": Tier,
                "MaxDepth": MaxDepth,
                "DailyDepth": dailyDepth,
                "Upgrades": Upgrades
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
