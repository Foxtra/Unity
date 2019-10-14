using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveBlock : MonoBehaviour {

	public string blockStatus = "free"; //статус блока

	public Transform edgeParticles; //переменная для эффектов

	public bool checkPlacement = false; //флаг, говорящий, что выбранный блок хотят переместить

	private Vector2 basePlace; //исходные координаты объекта

	GameObject[] emptyBlocks; //массив содержащий пустые блоки    
	GameObject closest = null; //указывает на ближайший пустой блок
	float nearest; //минимальное расстояние до ближайшего блока

	// Use this for initialization
	void Start () {
		emptyBlocks = GameObject.FindGameObjectsWithTag("empty"); //получаем массив пустых блоков в начале
		GenerateField(); //генерируем поле
		SetDistance(); //получаем расстояние до ближайшего блока
	}
	
	// Update is called once per frame
	void Update () {

		if (blockStatus == "pickedup") //статус блока говорит о том, что его перемещают
		{
			Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //получить координаты мышки
			Vector2 objPosition = Camera.main.ScreenToWorldPoint(mousePosition); //преобразовать в коодринаты юнити
			transform.position = objPosition; //передать координаты перетаскиваемому блоку

			if (Input.GetMouseButtonDown(1)) //если нажали прав клавишу мыши
			{
				transform.position = basePlace; //возвращаем блок на исходное место
				checkPlacement = false; //меняем флаг повторного нажатия мышки
				blockStatus = "free"; //блок больше не перемещается
				transform.GetComponent<SpriteRenderer>().sortingOrder = 1; //ставим порядок на слое
			}
		}

		GameObject go = GameObject.Find("edgeglow(Clone)");
		//удаление копий эффектов
		if (go)
		{
			Destroy(go.gameObject, 2f);
		}

}

	private void OnTriggerStay2D(Collider2D collision) //событие перекрытия "пустого" блока
	{
		
		float Distance = Vector2.Distance(collision.transform.position, basePlace); //рассчёт расстояния между исходной позицией и блоком, с которым происходит коллизия

		if (checkPlacement && (collision.gameObject.tag == "empty")) //если передвигаем объект на свободное место и нажали мышку
		{
			//если расстояние одинаковое, а также блок перемещается по диагоняли или вертикали
			if ((Math.Abs(nearest - Distance) < 1) && 
				((collision.transform.position.x == basePlace.x && collision.transform.position.y != basePlace.y) ||
				(collision.transform.position.y == basePlace.y && collision.transform.position.x != basePlace.x)))
			{
				transform.position = collision.gameObject.transform.position; //меняем
				collision.gameObject.transform.position = basePlace; //местами два объекта
				Instantiate(edgeParticles, transform.position, edgeParticles.rotation); //включаем эффект
				checkPlacement = false; //меняем флаг повторного нажатия мышки
				blockStatus = "free"; //блок больше не перемещается
				transform.GetComponent<SpriteRenderer>().sortingOrder = 1; //выставляем порядок на слое
				//Debug.Log(CheckWin());
				if (CheckWin())
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
	}

	private void OnMouseDown() //событие обработки нажатия мышки
	{
		if (Input.GetMouseButtonDown(0)) //если нажали лев клавишу мыши
		{   //если нажали не на "кирпич" и не на "пустой" блок, а также статус блока не говорит о том, что его перемещают
			if (transform.gameObject.tag != "block" && transform.gameObject.tag != "empty" && blockStatus != "pickedup")
			{
				blockStatus = "pickedup"; //придаём блоку статус перемещения
				basePlace = transform.position; //запоминаем координаты исходного объекта
				FindClosestDistance(transform.position); //запускаем поиск ближайшего "пустого" блока
				transform.GetComponent<SpriteRenderer>().sortingOrder = 10; //выставляем порядок в слое для того, чтобы блок был на переднем плане
			}
			else if (blockStatus == "pickedup") //если блок перемещается и нажата мышка
			{
				checkPlacement = true; //выставляем флаг, повторного нажатия мышки
			}
		}
	}

	GameObject FindClosestDistance(Vector2 position) //функция поиска ближайшего "пустого" блока
	{
		float distance = Mathf.Infinity; //начальное значение расстояния; 
		foreach (GameObject block in emptyBlocks) //цикл по всем объектам в массиве
		{
			Vector2 tmp = block.transform.position; //преобразование в вектор 2, т.к. по умолчанию block содержит координаты в вектор 3
			Vector2 diff = tmp - position;
			float curDistance = diff.sqrMagnitude; 
			if(curDistance < distance) //если вычислинное расстояние меньше начального
			{
				closest = block; //записали ближайший объект
				distance = curDistance; //обновили расстояние
			} 
		}
		return closest;
	}

	bool CheckWin() //проверка победы
	{   //массивы объектов по тэгам
		GameObject[] greenBlocks = GameObject.FindGameObjectsWithTag("green");
		GameObject[] yellowBlocks = GameObject.FindGameObjectsWithTag("yellow");
		GameObject[] redBlocks = GameObject.FindGameObjectsWithTag("red");

		//переменные на победу
		 bool checkGreen = true; 
		 bool checkYellow = true;
		 bool checkRed = true;

		GameObject gObj = greenBlocks[0];//присваиваем изначальное значение и будем с ним сверять остальные
		//float gValue = 358.2f;
		//Debug.Log("проверка зелёных: ");
		foreach (GameObject gBlock in greenBlocks) //цикл по всем объектам
		{   
			if(Math.Abs(gBlock.transform.position.x - gObj.transform.position.x) > 50)
			{
				checkGreen = false; //если хоть один объект не в столбике
				break; //ставим ложь и прекращаем цикл
			}
			//Debug.Log(gBlock.name + "--" + gBlock.transform.position.x);
		}
		GameObject yObj = yellowBlocks[0];
		//float yValue = 585.4f;
		//Debug.Log("проверка жёлтых: ");
		foreach (GameObject yBlock in yellowBlocks)
		{
			//Debug.Log(yBlock.name + "--" + yBlock.transform.position.x);
			if (Math.Abs(yBlock.transform.position.x - yObj.transform.position.x) > 50)
			{
				checkYellow = false;
				break;
			}
			
		}
		GameObject rObj = redBlocks[0];
		//float rValue = 812.6f;
		//Debug.Log("проверка красных: ");
		foreach (GameObject rBlock in redBlocks)
		{
			//Debug.Log(rBlock.name + "--" + rBlock.transform.position.x);
			if (Math.Abs(rBlock.transform.position.x - rObj.transform.position.x) > 50)
			{
				checkRed = false;
				break;
			}
		}
		if (checkGreen && checkYellow && checkRed)
			return true;
		else
			return false;
	}

	void GenerateField() //генерация поля
	{   //получаем массивы блоков, которые будем передвигать
		GameObject[] greenBlocks = GameObject.FindGameObjectsWithTag("green");
		GameObject[] yellowBlocks = GameObject.FindGameObjectsWithTag("yellow");
		GameObject[] redBlocks = GameObject.FindGameObjectsWithTag("red");

		//вычисляем суммарный размер массива
		int size = greenBlocks.Length + yellowBlocks.Length + redBlocks.Length; 

		//соединяем массивы в один
		GameObject[] allBlocks = new GameObject[size];
		greenBlocks.CopyTo(allBlocks, 0);
		yellowBlocks.CopyTo(allBlocks, greenBlocks.Length);
		redBlocks.CopyTo(allBlocks, greenBlocks.Length + yellowBlocks.Length);

		System.Random rand = new System.Random();

		//для каждого элемента в общем массиве будем генерировать с каким блоком он поменяется координамами
		foreach (GameObject gObj in allBlocks)
		{   //меняем се кроме блоков, указывающих порядок расстановки
			if(gObj.name == "yellow" || gObj.name == "red" || gObj.name == "green")
				continue;
			int rndValue = rand.Next(0, size);
			if (allBlocks[rndValue].name == "yellow" || allBlocks[rndValue].name == "red" || allBlocks[rndValue].name == "green")
				continue;
			Vector2 tmp = gObj.transform.position;
			gObj.transform.position = allBlocks[rndValue].transform.position;
			allBlocks[rndValue].transform.position = tmp;
		}    

	}

	//вычисление минимального расстояния между блоком и "пустым" блоком
	void SetDistance()
	{   //так как блоки "кирпичи" и "пустые" не меняют своего положения, ищем блок
		GameObject[] block = GameObject.FindGameObjectsWithTag("block");

		//получаем ближайший "пустой" блок и подсчитываем расстояние между ними
		closest = FindClosestDistance(block[0].transform.position);
		nearest = Vector2.Distance(closest.transform.position, block[0].transform.position);

	}

}