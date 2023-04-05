using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter,IHasProgress
{
    


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgresChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeScriptableObj[] FryingRecipeScriptableObjArray;
    [SerializeField] private BurningRecipeScriptableObj[] BurningRecipeScriptableObjArray;

    private State _state;
    private float _fryingTimer;
    private float _burningTimer;
    private FryingRecipeScriptableObj FryingRecipeScriptableObj;
    private BurningRecipeScriptableObj BurningRecipeScriptableObj;
    private void Start()
    {
        _state = State.Idle;      
    }
    private void Update()
    {
        if (HasKitchenObject())
        {   
            switch (_state)
          {
            case State.Idle:
                break;
            case State.Frying:
                _fryingTimer += Time.deltaTime;

                    OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = _fryingTimer / FryingRecipeScriptableObj.fryingTimerMax
                    });

                    if (_fryingTimer > FryingRecipeScriptableObj.fryingTimerMax)
                {
                  //Fried
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(FryingRecipeScriptableObj.Output, this);
          
                      
                        _state = State.Fried;
                        _burningTimer = 0f;
                        BurningRecipeScriptableObj = GetBurningRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObjectScriptableObj());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = _state
                        }); ;
                }
                break;
            case State.Fried:
                    _burningTimer += Time.deltaTime;
                    OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = _burningTimer / BurningRecipeScriptableObj.burningTimerMax
                    });

                    if (_burningTimer > BurningRecipeScriptableObj.burningTimerMax)
                    {
                        //Fried
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(BurningRecipeScriptableObj.Output, this);
                  
                        _state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = _state
                        });

                        OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        }); 
                    }
                    break;
                    
            case State.Burned:
                    break;
           
          }
      
           
        }  
    }
    public override void Interact(PlayerController player)
    {

        if (!HasKitchenObject())
        {
            //There is no kitchen object here
            if (player.HasKitchenObject())
            {//Player is carrying something
                if (HasRecipeWithInpuut(player.GetKitchenObject().GetKitchenObjectScriptableObj()))
                {//Player carrying something thath can be Fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    FryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(GetKitchenObject().GetKitchenObjectScriptableObj());
                    _state = State.Frying;
                    _fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = _state
                    });
                    OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = _fryingTimer / FryingRecipeScriptableObj.fryingTimerMax
                    }); 
                }

            }
            else
            {//Player not carriying anything

            }
        }
        else
        {
            //There is a kitchen object
            if (player.HasKitchenObject())
            {//Player is carrying something
                if (HasKitchenObject())
                {
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                    {//Player is holding a plate

                        if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectScriptableObj()))
                        {
                            GetKitchenObject().DestroySelf();
                            _state = State.Idle;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = _state
                            });

                            OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = 0f
                            });
                        }

                    }
                }
            }
            else
            {//Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
                _state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = _state
                });

                OnProgresChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    private bool HasRecipeWithInpuut(KitchenObjectScriptableObj inputKitchenObjectScriptableObj)
    {
        FryingRecipeScriptableObj fryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(inputKitchenObjectScriptableObj);
        return fryingRecipeScriptableObj != null;
    }
    private KitchenObjectScriptableObj GetOutputForInput(KitchenObjectScriptableObj inputKitchenObjectScriptableObj)
    {
        FryingRecipeScriptableObj fryingRecipeScriptableObj = GetFryingRecipeScriptableObjWithInput(inputKitchenObjectScriptableObj);
        if (fryingRecipeScriptableObj != null)
        {
            return fryingRecipeScriptableObj.Output;
        }
        else
        {
            return null;
        }

    }
    private FryingRecipeScriptableObj GetFryingRecipeScriptableObjWithInput(KitchenObjectScriptableObj inputKitchenObjectScriptableObj)
    {
        foreach (FryingRecipeScriptableObj fryingRecipeSO in FryingRecipeScriptableObjArray)
        {
            if (fryingRecipeSO.Input == inputKitchenObjectScriptableObj)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }  private BurningRecipeScriptableObj GetBurningRecipeScriptableObjWithInput(KitchenObjectScriptableObj inputKitchenObjectScriptableObj)
    {
        foreach (BurningRecipeScriptableObj burningRecipeScriptableObj in BurningRecipeScriptableObjArray)
        {
            if (burningRecipeScriptableObj.Input == inputKitchenObjectScriptableObj)
            {
                return burningRecipeScriptableObj;
            }
        }
        return null;
    }
    public bool IsFried()
    {
        return _state == State.Fried;
    }
}
