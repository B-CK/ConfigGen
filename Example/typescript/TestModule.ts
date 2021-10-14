import { Stream } from '../Stream';
import * as AllType from './AllType';
/**************************************** 数据配置表 *****************************************/
/** Test表数据类*/
export class TestCfg extends Stream {
	static readonly refence = 'TestModuleTestCfg';
	get length() { return this._cfgs ? this._cfgs.length : 0; }
	get cfgs() { return this._cfgs; }
	private _cfgs: Test[] = [];
	private _key2idx: Map<number, number> = new Map();
	constructor(rootDir: string) {
		super(rootDir + 'testmodule/test.da');
	}
	/**key索引数据(主键:TID)*/
	Get(key: number): Test | undefined {
		let idx = this._key2idx.get(key);
		if (idx == undefined) {
			console.error(`${this.path} key does not exist:${key}`);
			return undefined;
		}
		if (this.hasLoaded) {
			return this._cfgs[idx];
		}
		else {
			this.LoadConfig();
			return this._cfgs[idx];
		}
	}
	/**下标索引数据*/
	At(idx:number){
		if (this.hasLoaded) {
			return this._cfgs[idx];
		}
		else {
			this.LoadConfig();
			return this._cfgs[idx];
		}
	}
	protected ParseConfig() {
		this._cfgs = this.GetList('CfgTestModuleTest');
		for (let index = 0; index < this._cfgs.length; index++) {
			const item = this._cfgs[index];
			this._key2idx.set(item.TID, index);
		}
	}
	[Symbol.iterator]() { return this._cfgs.values(); }
}
export let _CFG_CLASS_ = [TestCfg];

/**************************************** 数据结构定义 *****************************************/

/**  */
export class TClass {
	/** Var1 */
	readonly Var1: string;
	/** Var2 */
	readonly Var2: number;
	constructor(o: any) {
		this.Var1 = o.GetString();
		this.Var2 = o.GetFloat();
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgTestModuleTClassMaker', {
	value: function (this: any) { return this[`Get${this.GetString().replace(/[\.]+/g, '')}`](); },
	writable: false,
});
Object.defineProperty(Stream.prototype, 'GetCfgTestModuleTClass', {
	value: function(this: any) { return new TClass(this); },
	writable: false,
});
/**  */
export class TM1 extends TClass {
	/** 继承1 */
	readonly V3: number;
	constructor(o: any) {
		super(o);
		this.V3 = o.GetLong();
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgTestModuleTM1', {
	value: function(this: any) { return new TM1(this); },
	writable: false,
});
/**  */
export class TM2 extends TClass {
	/** 继承2 */
	readonly V4: boolean;
	constructor(o: any) {
		super(o);
		this.V4 = o.GetBool();
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgTestModuleTM2', {
	value: function(this: any) { return new TM2(this); },
	writable: false,
});
/**  */
export class Test {
	/** 继承2 */
	readonly TID: number;
	/** 继承2 */
	readonly Name: string;
	/** 外部模块类型 */
	readonly Card: AllType.CardElement;
	constructor(o: any) {
		this.TID = o.GetInt();
		this.Name = o.GetString();
		this.Card = <AllType.CardElement>o.GetInt();
	}
}
Object.defineProperty(Stream.prototype, 'GetCfgTestModuleTest', {
	value: function(this: any) { return new Test(this); },
	writable: false,
});

/**************************************** 声明与导出 *****************************************/
declare module '../CfgManager' {
	interface CfgManager {
		get TestModuleTestCfg(): TestCfg;
	}
}

declare module '../Stream' {
	interface StreamBase {
		GetTClass(): TClass;
		GetTM1(): TM1;
		GetTM2(): TM2;
	}
}

