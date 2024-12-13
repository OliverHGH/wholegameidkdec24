using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsMethods 
{
    public Vector2 normaliser(Vector2 vtnorm)
    {
        float dist = Mathf.Sqrt(vtnorm.x * vtnorm.x + vtnorm.y * vtnorm.y);
        Vector2 final = new Vector2(vtnorm.x / dist, vtnorm.y / dist);
        return final;

    }
    public float findDif(Vector3 a, Vector3 b)
    {
        float xdif = a.x - b.x;
        float zdif = a.z - b.z;
        return (Mathf.Sqrt(xdif * xdif + zdif * zdif));
    }
    public bool islookingat(Transform a, Transform b, int maxanglex, int maxangley, LayerMask walls)
    {

        bool PathClear()
        {
            Vector3 Eright = b.position + a.transform.right * 0.7f;
            Vector3 ELeft = b.position - a.transform.right * 0.7f;
            Vector3 enemytop = b.position;
            Vector3 enemybottom = b.position;
            enemytop.y += 1.5f;
            enemybottom.y -= 1.4f;

            bool E = cansee(a.position, b.position - a.transform.forward * 0.7f);
            bool anyFalse =!E;
            return anyFalse;
        }

        bool cansee(Vector3 one ,Vector3 two)
        {
        return Physics.Linecast(one, two, walls);
         }
        bool ObservedY()
       {
        float eypos = b.position.y;
        float bottom = eypos - 1.5f;
        float top = eypos + 1.5f;
        float dif = findDif(a.position, b.position);
        float topdif = (top-a.position.y);
        float topangle= Mathf.Rad2Deg * Mathf.Atan(topdif / dif);
        float angle = a.transform.eulerAngles.x;
        
        if (angle <= 360 && angle >= 270)
        {
            angle = ((270 - angle) + 90);

        }
        else
        {
            angle = -angle;
        }
        if (Mathf.Abs(angle - topangle) < maxangley + (8/dif))
        {
            return true;
        }
        float botdif = (a.position.y-bottom);
        float botangle =-( Mathf.Rad2Deg * Mathf.Atan(botdif / dif));
        if (Mathf.Abs(angle - botangle) < maxangley + (8 / dif))
        {
            return true;
        }
        
        return false;

    }

        bool ObservedX()
        {
        Vector2 pp = new Vector2(a.position.x, a.position.z);
        Vector2 ep = new Vector2(b.position.x, b.position.z);
        Vector2 dahead = new Vector2(a.forward.x, a.forward.z);

        dahead = normaliser(dahead);
        pp = normaliser(ep - pp);
        float dotProduct = (dahead.x * pp.x + dahead.y * pp.y);
        float angle = Mathf.Rad2Deg * Mathf.Acos(dotProduct);
        float dif= (findDif(a.position, a.position));
        if (angle < maxanglex + 20/dif)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        return PathClear() && ObservedX() && ObservedY();
    }


}
