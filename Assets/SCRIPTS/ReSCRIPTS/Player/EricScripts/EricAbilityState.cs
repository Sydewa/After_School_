using UnityEngine;
using UnityEngine.InputSystem;

public class EricAbilityState : BaseState
{
    Eric_LupaScript _lupa;
    float elapsedTime;
    float damageInterval = 0.1f;
    float timeSinceLastDamage;
    public override void EnterState(IStateManager character)
    {
        _lupa = character.Character.GetComponentInChildren<Eric_LupaScript>();
        _lupa.StartCoroutine(_lupa.OnLupaStart());
        character.Animator.SetBool("OnAbility", true);
        elapsedTime = 0f;
        timeSinceLastDamage = 0f;
    }
    
    public override void UpdateState(IStateManager character)
    {
        //Para que las estadisticas extras de los objetos tengan efecto se tienen que anyadir los keyframes de la curva manualemnte
        //Primero se anyade el tiempo y luego la variable de (en este caso) danyo

        elapsedTime += Time.deltaTime;
        AnimationCurve damageCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 4f, 0f, 0.5f), new Keyframe(3f, 220f + (float)character.Power, 0f, 0f, 0f, 0f));
        float damageFromCurve = damageCurve.Evaluate(elapsedTime);

        int currentDamage = (int)damageFromCurve;
        int dmg = Mathf.CeilToInt((float) currentDamage / 10);

        timeSinceLastDamage += Time.deltaTime;
        

        //---------------------------------------
        
        //Creamos un Raycast como el que ya hemos hecho, de la camara hacia donde esta apuntando el raton en la escena, y luego creamos otro que va del centro de la lupa hasta el hit.point del primer  Raycast
        //HACER PARTICULAS O EL RAYO VISIBLE
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Rotacion del personaje
            Vector3 directionRotation = hit.point - character.Character.transform.position;
            Quaternion rotation = Quaternion.LookRotation(directionRotation);
            character.Character.transform.rotation = 
                Quaternion.Slerp(character.Character.transform.rotation, 
                Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), 
                Time.deltaTime * 18f);

            //Segundo Ray
            Vector3 direction = hit.point - _lupa.abilityPosition.position;
            Ray ray2 = new Ray(_lupa.abilityPosition.position, direction);
            RaycastHit hit2;
            if(Physics.Raycast(ray2, out hit2))
            {
                if(hit2.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && timeSinceLastDamage >= damageInterval)
                {
                    EnemyDamaged _enemyDamaged = hit.collider.GetComponent<EnemyDamaged>();
                    _enemyDamaged.OnEnemyDamaged(dmg);
                    timeSinceLastDamage = 0;
                    //Debug.Log(dmg);   
                }
                
                
                //---------------- HACER DAÑO UNA VEZ TENGA EL SCRIPT DE LOS ENEMIGOS
                Debug.DrawRay(ray2.origin, ray2.direction * hit2.distance, Color.red);
            }
        }

        /*if(!character.isAbilityPressed)
        {
            character.GoIdle();
        }*/
    }

    public override void ExitState(IStateManager character)
    {
        _lupa.StartCoroutine(_lupa.OnLupaEnd());
        character.Animator.SetBool("OnAbility", false);
    }
}
