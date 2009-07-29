// This is the main DLL file.

#include "stdafx.h"

#include "Touchy.h"

Touchy::TouchyFeely::TouchyFeely()
{}
Touchy::TouchyFeely::~TouchyFeely()
{}
Touchy::TouchyFeely::!TouchyFeely()
{}

System::Boolean Touchy::TouchyFeely::InitializeTouchPad()
{
	pin_ptr<ISynAPI*> p_pAPI = &m_pAPI;
	pin_ptr<ISynDevice*> p_pDevice = &m_pDevice;

	HRESULT result = SynCreateAPI(p_pAPI);

	if (m_pAPI)
	{
		// Find a TouchPad.
		long lHandle = -1;

		if (!(*p_pAPI)->FindDevice(SE_ConnectionAny, SE_DeviceTouchPad, &lHandle))
		{
			(*p_pAPI)->CreateDevice(lHandle, p_pDevice);
		}
	}

  return !!m_pDevice;
}