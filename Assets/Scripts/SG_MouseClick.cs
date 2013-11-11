using UnityEngine;
using System.Collections;

public class SG_MouseClick : MonoBehaviour {
	public bool isSMTP = false;
	
	void OnMouseUp() {
		//send email on click
		SG_Email sg_emailComponent = Camera.main.GetComponent("SG_Email") as SG_Email;
		if(isSMTP)
			sg_emailComponent.SendSendgridEmailSMTP();
		else
			sg_emailComponent.SendSendgridEmailWebAPI();
	}
	
}
