using System.Collections;
using System.Collections.Generic;

using Ink.Runtime;

using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    public class CommandLineParserAction
    {
        private int choiceIndex;

        public CommandLineParserAction()
        {
            choiceIndex = -1;
        }

        public void DoContinue() {
            choiceIndex = -1;
        }

        public void TakeChoice(int choiceIndex)
        {
            this.choiceIndex = choiceIndex;
        }

        public bool Continue => choiceIndex < 0;

        public int ChoiceIndex
        {
            get
            {
                if(choiceIndex < 0)
                {
                    throw new System.Exception("This action wants to continue");
                }
                return choiceIndex;
            }
        }
    }
}
