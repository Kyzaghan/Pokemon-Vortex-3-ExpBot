# -*- coding: utf-8 -*-

from Util.SettingsReader import read_config
from Util.Logger import logger


class Inventory:
    def __init__(self):
        self.c = read_config()
        self.Pokeball = 0
        self.GreatBall = 0
        self.UltraBall = 0
        self.MasterBall = 0
        self.l = logger()

    def getCurrentPokeBallCount(self):
        if self.c["Catcher"]["PokeBall"] == "Poke Ball":
            return self.Pokeball
        elif (self.c["Catcher"]["PokeBall"] == "Great Ball"):
            return self.GreatBall
        elif (self.c["Catcher"]["PokeBall"] == "Ultra Ball"):
            return self.UltraBall
        else:
            return self.MasterBall

    def removeCurrentPokeBallCount(self, IsLegend):
        if (IsLegend):
            self.MasterBall -= 1
        else:
            if self.c["Catcher"]["PokeBall"] == "Poke Ball":
                self.Pokeball -= 1
            elif (self.c["Catcher"]["PokeBall"] == "Great Ball"):
                self.GreatBall -= 1
            elif (self.c["Catcher"]["PokeBall"] == "Ultra Ball"):
                self.UltraBall -= 1
            else:
                self.MasterBall -= 1

    def getPokeBallBuyCount(self, type):
        try:
            if (type == "Poke Ball" and int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) > 0):
                PokeBallDiff = int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) - self.Pokeball
            elif (type == "Great Ball" and int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) > 0):
                PokeBallDiff = int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) - self.GreatBall
            elif (type == "Ultra Ball" and int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) > 0):
                PokeBallDiff = int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) - self.UltraBall
            elif (type == "Master Ball" and int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) > 0):
                PokeBallDiff = int(self.c["Catcher"]["PokeBallBuyList"]["" + type + ""]) - self.MasterBall
            else:
                PokeBallDiff = 0
            if (PokeBallDiff <= 0):
                return 0
            else:
                if PokeBallDiff >= 100 or PokeBallDiff > 50:
                    return 100
                elif 25 < PokeBallDiff <= 50:
                    return 50 * 2
                elif 10 < PokeBallDiff <= 25:
                    return 25 * 2
                elif 5 < PokeBallDiff <= 10:
                    return 10 * 2
                else:
                    return 5 * 2
        except Exception as e:
            self.l.writelog(str(e), "critical")


class Trainer:
    def __init__(self):
        self.inventory = Inventory()