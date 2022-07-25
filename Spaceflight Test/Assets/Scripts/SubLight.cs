using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
public class SubLight : MonoBehaviour
{
    public VisualEffect warpSpeedVFX;
    private bool warpActive = false;
    [SerializeField, Range(0f,1f)]
    public float rate = 0.02f;
    public MeshRenderer cylinder;
    public float delay = 2.5f;
    // Start is called before the first frame update
    void Awake()
    {
        warpSpeedVFX = GetComponent<VisualEffect>();
        warpSpeedVFX.Stop();
        warpSpeedVFX.SetFloat("Warp Amount", 0);
        cylinder.material.SetFloat("Active_", 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void onWarp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            warpActive = !warpActive;
            StartCoroutine(ActivateParticles());
            StartCoroutine(ActivateShader());
        }
        
    }

    IEnumerator ActivateParticles()
    {
        
        if (warpActive)
        {
            
            warpSpeedVFX.Play();
            float amount = warpSpeedVFX.GetFloat("Warp Amount");
            while(amount < 1 && warpActive)
            {
                amount += rate;
                warpSpeedVFX.SetFloat("Warp Amount", amount);
                yield return new WaitForSeconds(0.1f);
            }
        } else
        {
            float amount = warpSpeedVFX.GetFloat("Warp Amount");
            while (amount > 0 && !warpActive)
            {
                amount -= rate;
                warpSpeedVFX.SetFloat("Warp Amount", amount);
                yield return new WaitForSeconds(0.1f);

                if (amount <= 0 + rate)
                {
                    amount = 0;
                    warpSpeedVFX.SetFloat("Warp Amount", amount);
                    warpSpeedVFX.Stop();
                }
            }
            
        }
    }

    IEnumerator ActivateShader()
    {
        if (warpActive)
        {
            yield return new WaitForSeconds(delay);
            float amount = cylinder.material.GetFloat("Active_");
            while (amount < 1 && warpActive)
            {
                amount += rate;
                cylinder.material.SetFloat("Active_", amount);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            float amount = cylinder.material.GetFloat("Active_");
            while (amount > 0 && !warpActive)
            {
                amount -= rate;
                cylinder.material.SetFloat("Active_", amount);
                yield return new WaitForSeconds(0.1f);

                if (amount <= 0 + rate)
                {
                    amount = 0;
                    cylinder.material.SetFloat("Active_", amount);
                }
            }

        }
    }

}
