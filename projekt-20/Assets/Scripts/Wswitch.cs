using UnityEngine;

public class Wswitch : MonoBehaviour
{
    public int SelectedWeapon = 0;


    void Start()
    {
        SelectWeapon();
    }



    void Update()
    {
        int previusSelectedWeapon = SelectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f )
        {
            if (SelectedWeapon >= transform.childCount - 1)
                SelectedWeapon = 0;
            else
                SelectedWeapon++;


        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedWeapon <= 0)
                SelectedWeapon = transform.childCount - 1;
            else
                SelectedWeapon--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            SelectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            SelectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            SelectedWeapon = 3;
        }
        if (previusSelectedWeapon != SelectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == SelectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
