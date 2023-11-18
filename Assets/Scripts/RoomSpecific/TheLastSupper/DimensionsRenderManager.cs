
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DimensionsRenderManager : UdonSharpBehaviour
{
    //dimension numbers are set by their apperence in the inspector
    //public DimensionSorter[] sorters;
    [SerializeField] private GameObject dummy;
    [SerializeField] private MeshRenderer wipeoutMesh;
    [SerializeField] private Shader wipeoutShader;
    [SerializeField] private Shader bakeShader;
    [SerializeField] private Shader bakeFinal;

    private Material[] AddMaterial(Material[] data, Shader shader)
    {
	    GameObject tmp = Instantiate(dummy);
	    Material newMat = tmp.GetComponent<Renderer>().material;
	    newMat.shader = shader;
	    // TODO Destroy (tmp);
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
        InitializeDimensionQueue();
    }

	void InitializeDimensionQueue()
	{
		DimensionSorter[] sorters = GetComponentsInChildren<DimensionSorter>();
		//calculate total render space needed for the queue
		//first iteration requires 1 step less
		//last iteration bakes portals diffrently
		int currentQueue = 2000 - sorters.Length * 5 - 2; //1 for special, 1 to end in 1999

		for (int i = 0; i < sorters.Length; i++)
		{
			sorters[i].SetDiscardQueue(currentQueue);
			currentQueue++;
			wipeoutMesh.materials = AddMaterial(wipeoutMesh.materials, wipeoutShader);
			wipeoutMesh.materials[wipeoutMesh.materials.Length - 1].renderQueue = currentQueue;
			currentQueue++;
			if (i > 0)
			{
				// bake all previous dimensions
				for (int j = 0; j < i; j++)
				{
					for (int k = 0; k < sorters[j].portals.Length; k++)
					{
						MeshRenderer meshRenderer = sorters[j].portals[k].GetRenderer();
						meshRenderer.materials = AddMaterial(meshRenderer.materials, bakeShader);
						meshRenderer.materials[meshRenderer.materials.Length - 1].renderQueue = currentQueue;
					}
				}

				currentQueue++;
			}

			sorters[i].SetAcceptQueue(currentQueue);
			currentQueue++;
			for (int j = 0; j < sorters[i].portals.Length; j++)
			{
				MeshRenderer meshRenderer = sorters[i].portals[j].GetRenderer();
				meshRenderer.materials = AddMaterial(meshRenderer.materials, bakeShader);
				meshRenderer.materials[meshRenderer.materials.Length - 1].renderQueue = currentQueue;
			}
			currentQueue++;
		}

		wipeoutMesh.materials = AddMaterial(wipeoutMesh.materials, wipeoutShader);
		wipeoutMesh.materials[wipeoutMesh.materials.Length - 1].renderQueue = currentQueue;
		currentQueue++;

		for (int i = 0; i < sorters.Length; i++)
		{
			for (int j = 0; j < sorters[i].portals.Length; j++)
			{
				MeshRenderer meshRenderer = sorters[i].portals[j].GetRenderer();
				meshRenderer.materials = AddMaterial(meshRenderer.materials, bakeFinal);
				meshRenderer.materials[meshRenderer.materials.Length - 1].renderQueue = currentQueue;
			}
		}
	}
}