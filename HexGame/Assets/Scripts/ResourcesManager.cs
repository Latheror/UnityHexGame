using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour {


    // Make sure there is only one ResourcesManager
    public static ResourcesManager instance;

    void Awake()
    {   
        if(instance != null)
        {
            Debug.LogError("More than one ResourcesManager in scene !");
            return;
        }
        instance = this;
    }

    // Information about resources
    private Dictionary<string, ResourceInformation> resourcesInformation = new Dictionary<string, ResourceInformation>();

    // List of available resources and their caracteristics
    [SerializeField]
    public List<Resource> availableResources = new List<Resource>();

    // Dictionay of available resources and their caracteristics
    public Dictionary<string, Resource> availableResourcesDictionary = new Dictionary<string, Resource>();

    // Resources UI (top of the screen)
    public GameObject resourcesPanel1;
    public GameObject resourcesPanel2;
    public GameObject resourcePanelPrefab;

    public int nbIndicatorsPerPanel = 6;

    [SerializeField]
    private List<ResourceIndicator> resourcesIndicators = new List<ResourceIndicator>();

    public Dictionary<string, ResourceInformation> GetResourcesInformationDictionary(){ return resourcesInformation; }


	// Use this for initialization
	void Start () {

        // Now done in the StartManager
        //ImportResources();
        //BuildResourcesDictionary();
        //SetStartingResourcesAmounts();


	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Pay(BuildManager.BuildingType building)
    {
        foreach (var cost in building.resourcesCost)
        {
            resourcesInformation[cost.resource.name].SetResourceCurrentAmount(resourcesInformation[cost.resource.name].GetResourceCurrentAmount() - cost.amount);
            //resourcesAmounts[cost.resource.name].amount -= cost.amount;
        }

        // Update resources indicators
        UpdateResourcesIndicators();
    }


    public void IncreaseResource(Resource r, int changeAmount)
    {
        resourcesInformation[r.name].SetResourceCurrentAmount(resourcesInformation[r.name].GetResourceCurrentAmount() + changeAmount);
        //resourcesAmounts[r.name].amount += changeAmount;
    }

    public void DecreaseResource(Resource r, int changeAmount)
    {
        resourcesInformation[r.name].SetResourceCurrentAmount(resourcesInformation[r.name].GetResourceCurrentAmount() - changeAmount);
        //resourcesAmounts[r.name].amount -= changeAmount;
    }


    // Returns true if all the buildings needed resources are availables
    public bool CanPay(BuildManager.BuildingType building)
    {
        bool canAfford = true;
        foreach (var cost in building.resourcesCost)
        {   
            Debug.Log("Testing if we have enough " + cost.resource.name);
            if(cost.amount >= resourcesInformation[cost.resource.name].GetResourceCurrentAmount())
            {
                Debug.Log("We don't have enough " + cost.resource.name + " to build this !");
                canAfford = false;
            }
        }
        return canAfford;
    }

    // STEP 1 - Definition of resources, their associated sprite and color.
    public void ImportResources()
    {
        availableResources.Add(new Resource(1, "gold", "currency used to purchase buildings", Resources.Load<Sprite>("coins"), Color.yellow));
        availableResources.Add(new Resource(2, "wood", "basic material used to build", Resources.Load<Sprite>("wood"), null));
        availableResources.Add(new Resource(3, "food", "workers eat food...", Resources.Load<Sprite>("apple"), null));
        availableResources.Add(new Resource(4, "stone", "building material obtained from quarries", Resources.Load<Sprite>("stone"), null));
        availableResources.Add(new Resource(5, "iron ore", "building material obtained from mines", Resources.Load<Sprite>("iron_ore"), null));
        availableResources.Add(new Resource(6, "wheat", "produces by farms", Resources.Load<Sprite>("wheat"), null));
        availableResources.Add(new Resource(7, "flour", "produces by windmills", Resources.Load<Sprite>("flour"), null));
        availableResources.Add(new Resource(8, "jewel", "precious resource", Resources.Load<Sprite>("jewel"), null));
        availableResources.Add(new Resource(9, "bread", "food made with flour in a bakery", Resources.Load<Sprite>("bread"), null));
    }

    // STEP 2 - Build the corresponding resources dictionary.
    public void FillResourcesDictionary()
    {
        foreach (var item in availableResources)
        {
            availableResourcesDictionary.Add(item.name, item);
        }
    }

    // STEP 3 - Build new objects containing the starting, max, and current amount of each object
    public void BuildResourcesInformation()
    {
        resourcesInformation.Add("gold", new ResourceInformation(availableResourcesDictionary["gold"], 5000, 2000));
        resourcesInformation.Add("wood", new ResourceInformation(availableResourcesDictionary["wood"], 5000, 2000));
        resourcesInformation.Add("food", new ResourceInformation(availableResourcesDictionary["food"], 5000, 2000));
        resourcesInformation.Add("stone", new ResourceInformation(availableResourcesDictionary["stone"], 5000, 2000));
        resourcesInformation.Add("iron ore", new ResourceInformation(availableResourcesDictionary["iron ore"], 5000, 2000));
        resourcesInformation.Add("wheat", new ResourceInformation(availableResourcesDictionary["wheat"], 5000, 2000));
        resourcesInformation.Add("flour", new ResourceInformation(availableResourcesDictionary["flour"], 5000, 2000));
        resourcesInformation.Add("jewel", new ResourceInformation(availableResourcesDictionary["jewel"], 5000, 2000));
        resourcesInformation.Add("bread", new ResourceInformation(availableResourcesDictionary["bread"], 5000, 2000));
    }

    // STEP 4
    public void BuildResourcesIndicators()
    {
        foreach (var resourceInfo in resourcesInformation)
        {
            Resource resourceUsed = resourceInfo.Value.GetResourceType();

            if(GetNbOfRessourcesIndicatorsInPanel(1) < nbIndicatorsPerPanel)
            {
                GameObject instantiatedResourcePanel = Instantiate(resourcePanelPrefab, resourcesPanel1.transform.position, Quaternion.identity);
                instantiatedResourcePanel.transform.SetParent(resourcesPanel1.transform, false);
                // instantiatedResourcePanel.transform.localScale= new Vector3(1, 1, 1);

                resourcesIndicators.Add(new ResourceIndicator(resourceUsed, instantiatedResourcePanel));
            }
            else
            {
                GameObject instantiatedResourcePanel = Instantiate(resourcePanelPrefab, resourcesPanel2.transform.position, Quaternion.identity);
                instantiatedResourcePanel.transform.SetParent(resourcesPanel2.transform, false);
                // instantiatedResourcePanel.transform.localScale= new Vector3(1, 1, 1);

                resourcesIndicators.Add(new ResourceIndicator(resourceUsed, instantiatedResourcePanel));
            }
        }
    }

    // STEP 5 : Update display of maximum resources amount for each resource indicators
    public void UpdateMaxResourceAmountsIndicators()
    {
        foreach (var indicator in resourcesIndicators)
        {
            // Resource
            Resource resource = indicator.GetResource();
            //Debug.Log("Updating resource: " + indicator.resourceType.name);
            indicator.SetMaxResourceValue(resource.GetMaxAmount());
        }
    }

    // STEP 6
    public void UpdateResourcesIndicators()
    {
        foreach (var indicator in resourcesIndicators)
        {
            // Resource
            Resource resource = indicator.GetResource();
            //Debug.Log("Updating resource: " + indicator.resourceType.name);
            indicator.SetResourceValue(resourcesInformation[resource.name].GetResourceCurrentAmount());
        }
    }

    public void UpdateMaxResourcesIndicators()
    {
        foreach(var indicator in resourcesIndicators)
        {
            indicator.SetMaxResourceValue(/* TODO */ 0);
        }
    }

    public int GetNbOfRessourcesIndicatorsInPanel(int panelNb)
    {
        int indicatorsNb = 0;
        if(panelNb == 1)
        {
            indicatorsNb = resourcesPanel1.transform.childCount;
        }
        else if(panelNb == 2)
        {
            indicatorsNb = resourcesPanel2.transform.childCount;
        }
        else
        {
            Debug.LogError("Trying to access a non existing resource panel.");
            indicatorsNb = 0;
        }
        return indicatorsNb;
    }

    public void ProduceResource(Building b, Production p)
    {
        if(b.attributedWorkersNb > 0 && HaveEnoughIngredients(p))
        {   
            // Does this ressource need ingredients ?
            if(p.NeedIngredients())
            {
                // How many resources can we afford to produce ?
                int numberCanProduce = NumberCanProduce(p, b.attributedWorkersNb);

                // Remove the ingredients needed to produce the resource
                consumeIngredients(p, numberCanProduce);

                //resourcesAmounts[p.GetProductionResource().name].amount += (numberCanProduce /* * b.nbWorkers */);
                IncreaseResource(p.GetProductionResource(), numberCanProduce);
                UIManager.instance.DoPlusAnimation(b, p.GetProductionResource(), numberCanProduce);
            }
            else
            {
                Debug.Log("Producing " + p.GetAmountByWorker() + " " + p.GetProductionResource().name.ToString() + ".");
                //resourcesAmounts[p.GetProductionResource().name].amount += (p.GetAmountByWorker() * b.attributedWorkersNb);
                IncreaseResource(p.GetProductionResource(), p.GetAmountByWorker() * b.attributedWorkersNb);
                UIManager.instance.DoPlusAnimation(b, p.GetProductionResource(), p.GetAmountByWorker() * b.attributedWorkersNb);
            }

        }

    }

    public bool HaveEnoughIngredients(Production p)
    {
        bool haveEnoughIngredients = true;
        if(p.GetIngredients() != null)
        {
            List<ResourceAmount> ingredients = p.GetIngredients();
            foreach (var ingredient in ingredients)
            {
                Debug.Log("Trying to see if we have enough " + ingredient.resource.name + " in order to produce " + p.GetProductionResource().name);
                //if(!(resourcesAmounts[ingredient.resource.name].amount >= ingredient.amount))
                if(! (resourcesInformation[ingredient.resource.name].GetResourceCurrentAmount() >= ingredient.amount))
                {
                    haveEnoughIngredients = false;
                    Debug.Log("We don't have enough " + ingredient.resource.name + " to produce " + p.GetProductionResource().name + " !");
                    InfoPanel.instance.SetInfo("We don't have enough " + ingredient.resource.name + " to produce " + p.GetProductionResource().name + " !");
                }
            }
        }
        return haveEnoughIngredients;
    }

    public int NumberCanProduce(Production p, int nbWorkers)
    {
        int numberCanProduce = 1;

        // TODO : HANDLE ALL THE POSSIBLE INGREDIENTS !
        foreach (var ingredient in p.GetIngredients())
        {
            int totalIngredientAmount = resourcesInformation[ingredient.resource.name].GetResourceCurrentAmount();
            int neededIngredientAmount = ingredient.amount * nbWorkers * p.GetAmountByWorker();
            //numberCanProduce = neededIngredientAmount / totalIngredientAmount;

            Debug.Log("NumberCanProduce : Total amount of " + ingredient.resource.name + ": " + totalIngredientAmount);
            Debug.Log("NumberCanProduce : We'd like to produce " + nbWorkers * p.GetAmountByWorker() + " units of " + p.GetProductionResource().name);
            Debug.Log("NumberCanProduce : Needed amount of " + ingredient.resource.name + ": " + neededIngredientAmount);

            if(neededIngredientAmount > totalIngredientAmount)
            {
                numberCanProduce = totalIngredientAmount / ingredient.amount;
            }
            else
            {
                numberCanProduce = p.GetAmountByWorker() * nbWorkers; 
            }

            Debug.Log("NumberCanProduce : We can produce " + numberCanProduce + " " + p.GetProductionResource().name);

        }

        return numberCanProduce;
    }

    // TODO : TAKE NUMBER OF WORKERS INTO ACOUNT
    public void consumeIngredients(Production p, int numberCanProduce)
    {
        if(p.GetIngredients() != null)
        {
            List<ResourceAmount> ingredients = p.GetIngredients();
            foreach (var ingredient in ingredients)
            {
                if(resourcesInformation[ingredient.resource.name].GetResourceCurrentAmount() >= ingredient.amount)
                //if(resourcesAmounts[ingredient.resource.name].amount >= ingredient.amount)
                {
                    DecreaseResource(resourcesInformation[ingredient.resource.name].GetResourceType(), ingredient.amount * numberCanProduce);
                }
                else
                {
                    Debug.LogError("ERROR - Consume ingredients");
                }
            }
        }
        else
        {
            Debug.LogError("ERROR - Consume ingredients");
        }
    }

    public void setMaxResourceAmount(Resource r, int max)
    {
        r.maxAmount = max;
        UpdateMaxResourceAmountsIndicators();
    }

    public void increaseMaxResourceAmount(Resource r, int increaseNb)
    {
        r.maxAmount += increaseNb;
        UpdateMaxResourceAmountsIndicators();
    }

    public void decreaseMaxResourceAmount(Resource r, int decreaseNb)
    {
        r.maxAmount -= decreaseNb;
        UpdateMaxResourceAmountsIndicators();
    }



    [System.Serializable]
    public class Resource
    {
        public int id;
        public string name;
        public string description;
        public Sprite image;
        public Color color;
        public int currentAmount;
        public int maxAmount = 10000;
        public Resource(int id, string name, string description = "noDescription", Sprite image = null, Color ? c = null)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.image = image;
            this.color = c ?? Color.black;
        }

        public int GetMaxAmount(){ return this.maxAmount; }
    }

    [System.Serializable]
    public class ResourceCost
    {
        public Resource resource;
        public int amount;

        public ResourceCost(Resource r, int a)
        {
            this.resource = r;
            this.amount = a;
        }
    }


    [System.Serializable]
    public class ResourceAmount
    {
        public Resource resource;
        public int amount;

        public ResourceAmount(Resource r, int a)
        {
            this.resource = r;
            this.amount = a;
        }
    }

    [System.Serializable]
    public class ResourceInformation
    {
        Resource resourceType;
        int resourceMaxAmount;
        int resourceStartAmount;
        int resourceCurrentAmount;

        public Resource GetResourceType(){ return this.resourceType; }
        public void SetResourceType(Resource resType){ this.resourceType = resType; }
        public int GetResourceMaxAmount(){ return this.resourceMaxAmount; }
        public void SetResourceMaxAmount(int maxAmount){ this.resourceMaxAmount = maxAmount; }
        public int GetResourceCurrentAmount(){ return this.resourceCurrentAmount; }
        public void SetResourceCurrentAmount(int amount){ this.resourceCurrentAmount = amount; }

        public ResourceInformation(Resource resType, int maxAmount, int startAmount)
        {
            this.resourceType = resType;
            this.resourceMaxAmount = maxAmount;
            this.resourceStartAmount = startAmount;
            this.resourceCurrentAmount = startAmount;
        }
    }


    [System.Serializable]
    public class ResourceIndicator
    {
        public string resourceName;

        public Resource resource;

        public int value;
        public GameObject panel;

        public Resource resourceType;

        public GameObject image;
        public GameObject resourceText;

        public GameObject resourceValue;
        public GameObject maxResourceValue;

        public ResourceIndicator(Resource res, GameObject go)
        {
            this.resource = res;
            this.resourceName = res.name;
            this.panel = go;

            ResourcePanel rPanel = this.panel.GetComponent<ResourcePanel>();

            this.resourceType = ResourcesManager.instance.availableResourcesDictionary[res.name];

            this.image = rPanel.image;
            this.resourceText = rPanel.nameGO;
            this.resourceValue = rPanel.valueGO;
            this.maxResourceValue = rPanel.maxValueGO;

            this.SetResourceText(res.name);
            this.SetResourceValue(0);
            this.SetMaxResourceValue(0);
            this.SetResourceImage(res.image);
        }

        public Resource GetResource(){ return this.resource; }

        public void SetResourceText(string text){ resourceText.GetComponent<Text>().text = text; }
        public void SetResourceValue(string value){ resourceValue.GetComponent<Text>().text = value; }
        public void SetResourceValue(int value){ resourceValue.GetComponent<Text>().text = value.ToString(); }
        public void SetMaxResourceValue(int value){ maxResourceValue.GetComponent<Text>().text = value.ToString(); }
        public void SetResourceImage(Sprite img){image.GetComponent<Image>().sprite = img; }
    }


    [System.Serializable]
    public class Production
    {
        private Resource productionResource;
        private int amountByWorker;
        private float productionDelay;
        private List<ResourceAmount> ingredients;
        private bool needIngredients;

        public Resource GetProductionResource()
        {
            return productionResource;
        }

        public int GetAmountByWorker()
        {
            return amountByWorker;
        }

        public List<ResourceAmount> GetIngredients()
        {
            return this.ingredients;
        }

        public bool NeedIngredients()
        {
            return this.needIngredients;
        }

        // TODO : OLD VERSION - TO DELETE
        public Production(Resource prodRes, int amountByWorker, float delay)
        {
            this.productionResource = prodRes;
            this.amountByWorker = amountByWorker;
            this.productionDelay = delay;
        }

        public Production(Resource prodRes, int amountByWorker, float delay, bool needIngredients, List<ResourceAmount> ingredients)
        {
            this.productionResource = prodRes;
            this.amountByWorker = amountByWorker;
            this.productionDelay = delay;
            this.ingredients = ingredients;
            this.needIngredients = needIngredients;
        }
    }

}


// TODO :
// Maybe make an enum for all available resources names