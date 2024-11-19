from sqlalchemy import JSON, Column, Float, Integer, Sequence, String
from sqlalchemy.orm import declarative_base

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

    def __export__(self):
        return {
            "UUID": self.UUID,
            "UserName": self.UserName,
            "Tier": self.Tier,
            "MaxDepth": self.MaxDepth,
            "DailyDepth": self.DailyDepth,
            "Upgrades": self.Upgrades,
        }

    def setParam(self, key, value):
        if key == "UUID":
            return False, "Cant change UUID"
        else:
            setattr(self, key, value)
            return True, ""
