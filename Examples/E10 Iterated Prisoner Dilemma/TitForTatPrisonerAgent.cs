/**************************************************************************
 *                                                                        *
 *  Website:     https://github.com/florinleon/ActressMas                 *
 *  Description: Iterated Prisoner's Dilemma using ActressMas framework   *
 *  Copyright:   (c) 2018, Florin Leon                                    *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

namespace IteratedPrisonersDilemma
{
    public class TitForTatPrisonerAgent : PrisonerAgent
    {
        protected override string ChooseAction(int lastOutcome)
        {
            if (lastOutcome == 0 || lastOutcome == -1)
                return "deny"; // initially, lastOutcome = 0 => cooperate first time

            if (lastOutcome == -3 || lastOutcome == -5)
                return "confess";

            return "";
        }
    }
}