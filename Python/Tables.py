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
    Currency = Column(JSON)
    ShopItems_Bought = Column(String)
    ShopItems_Locked = Column(String)

    def __repr__(self):
        return f"<User(UUID={self.UUID}, UserName='{self.UserName}', Tier={self.Tier}, MaxDepth={self.MaxDepth}, DailyDepth={self.DailyDepth}, Upgrades={self.Upgrades}, Currency={self.Currency}, ShopItems_Bought={ShopItems_Bought}, ShopItems_Locked={self.ShopItems_Locked}>"

    def __export__(self, uuid=True, UserName=True, Tier=True, MaxDepth=True, DailyDepth=True, Upgrades=True, Currency=True, ShopItems_Bought=True, ShopItems_Locked=True):
        return {
            key: getattr(self, key)
            for key, include in {
                "UUID": uuid,
                "UserName": UserName,
                "Tier": Tier,
                "MaxDepth": MaxDepth,
                "DailyDepth": DailyDepth,
                "Upgrades": Upgrades,
                "Currency": Currency,
                "ShopItems_Bought": ShopItems_Bought,
                "ShopItems_Locked": ShopItems_Locked
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

    ID = Column(Integer, Sequence("seed_id_seq"), primary_key=True)
    Date = Column(JSON,unique=True)
    Value = Column(Integer, unique=True)  # Enforce uniqueness for the Seed column

    def __repr__(self):
        return f"<Seed(Date='{self.Date}', Value={self.Value})>"

    def __export__(self, Date=True, Value=True, ID=True):
        return {
            key: getattr(self, key)
            for key, include in {
                "Date": Date,
                "Value": Value,
                "ID": ID
            }.items()
            if include
        }
        
class ShopItem(Base):
    __tablename__ = "ShopItem"

    ID = Column(Integer, Sequence("seed_id_seq"), primary_key=True)
    Name = Column(String)
    Description = Column(String)
    PreReq = Column(String)
    Locks = Column(String)
    Price = Column(JSON)
    Effect = Column(JSON)

    def __repr__(self):
        return f"<ShopItem(Name='{self.Name}', Description='{self.Description}', PreReq='{self.PreReq}', Locks='{self.Locks}', Price={self.Price}, Effect={self.Effect})>"

    def __export__(self, Name=True, Description=True, PreReq=True, Locks=True, Price=True, Effect=True, ID=True):
        return {
            key: getattr(self, key)
            for key, include in {
                "Name": Name,
                "Description": Description,
                "PreReq": PreReq,
                "Locks": Locks
                "Price": Price,
                "Effect": Effect,
                "ID": ID
            }.items()
            if include
        }