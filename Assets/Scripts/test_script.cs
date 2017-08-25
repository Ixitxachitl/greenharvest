using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*

else if (bulletType == BulletType.DoubleShot && currentDoubleShotAmmo > 0) {
            nextFire = Time.time + DoubleShotRate;
            var doubleShot1 = Instantiate (Projectile, ProjectilePos.position, Quaternion.identity);
            var doubleShot2 = Instantiate (Projectile, ProjectilePos.position, Quaternion.identity);
            Vector2 temp = ProjectilePos.transform.position;
            doubleShot2.transform.position = temp;
            doubleShot2.transform.Translate (FirstBulletTranslateX, FirstBulletTranslateY, 0, Space.World);
            doubleShot1.transform.position = temp;
            doubleShot1.transform.Translate (SecondBulletTranslateX, SecondBulletTranslateY, 0, Space.World);
            currentDoubleShotAmmo--;
        }

    */    public    class BT {
            public int DoubleShot;
        }
public class test_script : MonoBehaviour {

    private int bulletType;
    private int currentDoubleShotAmmo;
    private GameObject Projectile;
    private Transform ProjectilePos;
    private List<GameObject> shot;
    private bool firstBullet;


    // Use this for initialization
    void Start()
    {
        BT BulletType = new BT();
        firstBullet = true;
        if (bulletType == BulletType.DoubleShot && currentDoubleShotAmmo > 0)
        {
            for (int x = 0; x<=1; x++)
            {
                shot.Add(Instantiate(Projectile, ProjectilePos.position, Quaternion.identity));
            }
            foreach(GameObject bullet in shot)
            {

            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
