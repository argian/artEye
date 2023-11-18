
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DimensionsRenderManager : UdonSharpBehaviour
{
    //dimension numbers are set by their apperence in the inspector
    //public DimensionSorter[] sorters;
    public MeshRenderer WipeoutMesh;
    public Shader wipeoutShader;
    public int StartingQueue;

    private Material[] AddMaterial(Material[] data, Material newMat)
	{
        Material[] res = new Material[data.Length + 1];
        for (int i = 0; i < data.Length; i++)
		{
            res[i] = data[i];
		}
        res[res.Length - 1] = newMat;
        return res;
	}

	void Start()
    {
        DimensionSorter[] sorters = gameObject.GetComponentsInChildren<DimensionSorter>(includeInactive: false);
        //calculate total render space needed for the queue
        //first iteration requires 1 step less
        //last iteration bakes portals diffrently
        StartingQueue = 2000 - sorters.Length * 6 - 2; //1 for special, 1 to end in 1999

        //here goes 6 steps plan for renderQueue
        //discard queue
        //wipeout
        //bake all previous dimensions
        //accept queue
        //wipeout
        //bake all dimensions
        /*
        int currentQueue = StartingQueue;
        int dimensionLimit = 0;
        //int wipeoutIndex = 0;
        for (int i = 0; i < sorters.Length; i++)
        {
            //step 1
            sorters[i].DiscardQueue = currentQueue;
            currentQueue += 1;
            //step 2
            WipeoutMesh.materials = AddMaterial(WipeoutMesh.materials, new Material(wipeoutShader));
            WipeoutMesh.materials[WipeoutMesh.materials.Length - 1].renderQueue = currentQueue;
            currentQueue += 1;
            //step 3
            for (int j = 0; j < dimensionLimit; j++)
			{
                //DimensionSorter[j].
			}
            //step 4
            sorters[i].AcceptQueue = currentQueue;
            currentQueue += 1;
            //step 5
            WipeoutMesh.materials = AddMaterial(WipeoutMesh.materials, new Material(wipeoutShader));
            WipeoutMesh.materials[WipeoutMesh.materials.Length - 1].renderQueue = currentQueue;
            currentQueue += 1;
        }
        */
    }

}
