
using System;
using UnityEngine;

namespace TheTab.Runtime
{
    public class CreatTable : TheTable
    {
        #region Publics

        

        #endregion


        #region Unity Api

        private void Start()
        {
            CreateTable(table,numberToTable);
        }

        private void Update()
        {
            
        }

        #endregion


        #region Utils

        

        #endregion


        #region Main Methode
        

        #endregion
        
        
        #region Privates
        
        [SerializeField]private int numberToTable = 0;
        [SerializeField]private GameObject table;
        
        #endregion
    }
}
