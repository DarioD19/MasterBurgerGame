using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IKitchenObjectParent
{
    public static PlayerController Instance { get; private set; }

    public event EventHandler OnPickedSomething;


    public event EventHandler<OnSelectedCounterChangedArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float _speed;
    [SerializeField] private GameInput GameInput;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;
    
    private bool _isWalking;
    private Vector3 _lastInteractDir;
    private BaseCounter _SelectedCounter;
    private KitchenObject KitchenObject;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }
        Instance = this;
    }
    private void Start()
    {
        GameInput.OnInteractAction += GameInput_OnInteractAction;
        GameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

            if (GameManager.Instance.IsGamePlaying())
            {
               if (_SelectedCounter != null)
               {
                 _SelectedCounter.InteractAlternate(this);
               }
            }
      
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (GameManager.Instance.IsGamePlaying())
        {
            if (_SelectedCounter != null)
            {
                _SelectedCounter.Interact(this);
            }
        }
      
    }
      
            
    private void Update()
    {
        HandleMovement();
        HandeInteractions();
    }
    public bool IsWalking()
    {
        return _isWalking;
    }
    private void HandeInteractions()
    {
        Vector2 inputVector = GameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            _lastInteractDir = moveDirection;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance,_countersLayerMask))
        {
            //Checking if hited object has cpmponent
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {//Has clear counter
                if (baseCounter != _SelectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                   
                }
               
            }
            else
            {
                SetSelectedCounter(null);
               
            }


        }
        else
        {
            SetSelectedCounter(null);
        }


        
    }
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = _speed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);
        if (!canMove)
        {
            //Cannot move towards direction

            //Attempt only X movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;

            canMove =moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);
            if (canMove)
            {
                //Can move only on the X axis
                moveDirection = moveDirectionX;
            }
            else
            {
                //Cannot move only on the X

                //Attempt only Z movement
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);
                if (canMove)
                {
                    //Can move only on the Z axis
                    moveDirection = moveDirectionZ;
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }


        _isWalking = moveDirection != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this._SelectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedArgs
        {
            selectedCounter = selectedCounter
        }) ;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.KitchenObject = kitchenObject;
        if (KitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return KitchenObject;
    }

    public void ClearKitchenObject()
    {
        KitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return KitchenObject != null;
    }
   
  
  
   
}
