using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserUnitManager : MonoBehaviour
{
    [SerializeField] private GameObject UserUnit;
    [SerializeField] private int gapY = 100;

    public void SetUserUnit(List<UserData> users)
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }

        if (users == null) return;
        if (users.Count == 0) return;

        //users.Reverse();

        for (int i = 0; i < users.Count; i++)
        {
            GameObject userUnit = Instantiate(UserUnit, transform);
            UserUnitController controller = userUnit.GetComponent<UserUnitController>();
            controller.setDate(users[i]);

        }
    }
}
